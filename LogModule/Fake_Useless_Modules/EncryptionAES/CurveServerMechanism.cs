using NetMQServer.Core.Utils;
using System;
using System.Security.Cryptography;

namespace NetMQServer.Core.Mechanisms
{
    internal class CurveServerMechanism : CurveMechanismBase
    {
        protected enum State
        {
            WaitingForHello,
            SendingWelcome,
            SendingReady,
            WaitingForInitiate,
            Ready
        }

        private byte[] m_secretKey;
        private byte[] m_cnSecretKey;
        private byte[] m_cnPublicKey;
        private byte[] m_cnClientKey;
        private byte[] m_cookieKey;
        private State m_state;

        public CurveServerMechanism(SessionBase session, Options options) :
            base(session, options, "CurveZMQMESSAGES", "CurveZMQMESSAGEC")
        {
            m_secretKey = (byte[]) options.CurveSecretKey.Clone();
           
        }
        
        public override void Dispose()
        {
            base.Dispose();
            Array.Clear(m_secretKey, 0, 32);
            Array.Clear(m_cnSecretKey, 0, 32);
            Array.Clear(m_cnPublicKey, 0, 32);
            Array.Clear(m_cnClientKey, 0, 32);
            Array.Clear(m_cookieKey, 0, m_cookieKey.Length);
        }

        public override MechanismStatus Status
        {
            get
            {
                if (m_state == State.Ready)
                    return MechanismStatus.Ready;
                else
                    return MechanismStatus.Handshaking;
            }
        }

        public override PullMsgResult NextHandshakeCommand(ref Msg msg)
        {
            PullMsgResult result;

            switch (m_state)
            {
                case State.SendingWelcome:
                    result = ProduceWelcome(ref msg);
                    if (result == PullMsgResult.Ok)
                        m_state = State.WaitingForInitiate;
                    break;
                case State.SendingReady:
                    result = ProduceReady(ref msg);
                    if (result == PullMsgResult.Ok)
                        m_state = State.Ready;
                    break;
                default:
                    result = PullMsgResult.Empty;
                    break;
            }

            return result;
        }

        public override PushMsgResult ProcessHandshakeCommand(ref Msg msg)
        {
            PushMsgResult result;

            switch (m_state)
            {
                case State.WaitingForHello:
                    result = ProcessHello(ref msg);
                    break;
                case State.WaitingForInitiate:
                    result = ProcessInitiate(ref msg);
                    break;
                default:
                    return PushMsgResult.Error;
            }

            if (result == PushMsgResult.Ok)
            {
                msg.Close();
                msg.InitEmpty();
            }

            return result;
        }

        PushMsgResult ProcessHello(ref Msg msg)
        {
            if (!CheckBasicCommandStructure(ref msg))
                return PushMsgResult.Error;

            Span<byte> hello = msg;

            if (!IsCommand("HELLO", ref msg))
                return PushMsgResult.Error;

            if (hello.Length != 200)
                return PushMsgResult.Error;

            byte major = hello[6];
            byte minor = hello[7];

            if (major != 1 || minor != 0)
            {
                // client HELLO has unknown version number
                return PushMsgResult.Error;
            }

            // Save client's short-term public key (C')
            hello.Slice(80, 32).CopyTo(m_cnClientKey);

          
        
            m_peerNonce = NetworkOrderBitsConverter.ToUInt64(hello, 112);

          

            Span<byte> helloPlaintext = stackalloc byte[80];
         
     

            helloPlaintext.Clear();
            
            m_state = State.SendingWelcome;
            return PushMsgResult.Ok;
        }

        PullMsgResult ProduceWelcome(ref Msg msg)
        {
        

            //  Create full nonce for encryption
            //  8-byte prefix plus 16-byte random nonce
          
            using var rng = RandomNumberGenerator.Create();
#if NETSTANDARD2_1
            rng.GetBytes(cookieNonce.Slice(8));
#else
            byte[] temp = new byte[16];
            rng.GetBytes(temp);
           
#endif

            // Generate cookie = Box [C' + s'](t)
         
          
            //  Create full nonce for encryption
            //  8-byte prefix plus 16-byte random nonce
        
#if NETSTANDARD2_1
            rng.GetBytes(welcomeNonce.Slice(8));
#else
            rng.GetBytes(temp);
           
#endif

            // Create 144-byte Box [S' + cookie](S->C')
          

            return PullMsgResult.Ok;
        }

        PushMsgResult ProcessInitiate(ref Msg msg)
        {
            if (!CheckBasicCommandStructure(ref msg))
                return PushMsgResult.Error;

            Span<byte> initiate = msg;

            if (!IsCommand("INITIATE", ref msg))
                return PushMsgResult.Error;

            if (initiate.Length < 257)
                return PushMsgResult.Error;

           
            Span<byte> cookiePlaintext = stackalloc byte[64];
            Span<byte> cookieBox = initiate.Slice(25, 80);

          
         

            //  Check cookie plain text is as expected [C' + s']
            if (!SpanUtility.Equals(m_cnClientKey, cookiePlaintext.Slice(0, 32)) ||
                !SpanUtility.Equals(m_cnSecretKey, cookiePlaintext.Slice(32, 32)))
                return PushMsgResult.Error;

          
            byte[] initiatePlaintext = new byte[msg.Size - 113];
            var initiateBox = initiate.Slice(113);

          
          
          
         
            Span<byte> vouchPlaintext = stackalloc byte[64];
            Span<byte> vouchBox = new Span<byte>(initiatePlaintext, 48, 80);
            var clientKey = new Span<byte>(initiatePlaintext, 0, 32);

            

            //  What we decrypted must be the client's short-term public key
            if (!SpanUtility.Equals(vouchPlaintext.Slice(0, 32), m_cnClientKey))
                return PushMsgResult.Error;

            //  Create the session box
          

            //  This supports the Stonehouse pattern (encryption without authentication).
            m_state = State.SendingReady;

            if (!ParseMetadata(new Span<byte>(initiatePlaintext, 128, initiatePlaintext.Length - 128 - 16)))
                return PushMsgResult.Error;
            
            vouchPlaintext.Clear();
            Array.Clear(initiatePlaintext, 0, initiatePlaintext.Length);

            return PushMsgResult.Ok;
        }

        PullMsgResult ProduceReady(ref Msg msg)
        {
            int metadataLength = BasicPropertiesLength;
           
            byte[] readyPlaintext = new byte[metadataLength];

            //  Create Box [metadata](S'->C')
            AddBasicProperties(readyPlaintext);

        

            m_nonce++;

            return 0;
        }
    }
}