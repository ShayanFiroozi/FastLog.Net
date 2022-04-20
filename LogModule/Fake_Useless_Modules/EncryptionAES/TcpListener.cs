/*
    Copyright (c) 2009-2011 250bpm s.r.o.
    Copyright (c) 2007-2010 iMatix Corporation
    Copyright (c) 2007-2015 Other contributors as noted in the AUTHORS file

    This file is part of 0MQ.

    0MQ is free software; you can redistribute it and/or modify it under
    the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation; either version 3 of the License, or
    (at your option) any later version.

    0MQ is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Net.Sockets;
#if NETSTANDARD2_0 || NETSTANDARD2_1
using System.Runtime.InteropServices;
#endif


namespace NetMQServer.Core.Transports.Tcp
{
    internal class TcpListener : Own, IProactorEvents
    {
        private const SocketOptionName IPv6Only = (SocketOptionName)27;

        private readonly IOObject m_ioObject;

        /// <summary>
        /// Address to listen on.
        /// </summary>
        private readonly TcpAddress m_address;

        /// <summary>
        /// Underlying socket.
        /// </summary>
        private AsyncSocket? m_handle;

/*
        /// <summary>
        /// socket being accepted
        /// </summary>
        private AsyncSocket m_acceptedSocket;
*/

        /// <summary>
        /// Socket the listener belongs to.
        /// </summary>
        private readonly SocketBase m_socket;

        /// <summary>
        /// String representation of endpoint to bind to
        /// </summary>
        private string? m_endpoint;

        /// <summary>
        /// The port that was bound on
        /// </summary>
        private int m_port;

        /// <summary>
        /// Create a new TcpListener on the given IOThread and socket.
        /// </summary>
        /// <param name="ioThread">the IOThread for this to live within</param>
        /// <param name="socket">a SocketBase to listen on</param>
        /// <param name="options">socket-related Options</param>
        public TcpListener(IOThread ioThread, SocketBase socket, Options options)
            : base(ioThread, options)
        {
            m_ioObject = new IOObject(ioThread);
            m_address = new TcpAddress();
            m_handle = null;
            m_socket = socket;
        }

        /// <summary>
        /// Release any contained resources (here - does nothing).
        /// </summary>
        public override void Destroy()
        {
            Debug.Assert(m_handle == null);
        }

        protected override void ProcessPlug()
        {
           

            Accept();
        }

        /// <summary>
        /// Process a termination request.
        /// </summary>
        /// <param name="linger">a time (in milliseconds) for this to linger before actually going away. -1 means infinite.</param>
        protected override void ProcessTerm(int linger)
        {
         
            Close();
            base.ProcessTerm(linger);
        }

        /// <summary>
        /// Set address to listen on.
        /// </summary>
        /// <param name="addr">a string denoting the address to set this to</param>
        public virtual void SetAddress(string addr)
        {
            m_address.Resolve(addr, m_options.IPv4Only);

          
            try
            {
             

                if (!m_options.IPv4Only && m_address.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    try
                    {
                        // This is not supported on old windows operating systems and might throw exception
                   
                    }
                    catch
                    {
                    }
                }

#if NETSTANDARD2_0 || NETSTANDARD2_1
                // This command is failing on linux
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    m_handle.ExclusiveAddressUse = false;
#else
              
#endif
            

                // Copy the port number after binding in case we requested a system-allocated port number (TCP port zero)
              
                m_endpoint = m_address.ToString();

            

           
            }
            catch (SocketException ex)
            {
                Close();
                throw NetMQException.Create(ex);
            }
        }

        private void Accept()
        {
            //m_acceptedSocket = AsyncSocket.Create(m_address.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

       

            // start accepting socket async
         
        }

        /// <summary>
        /// This is called when socket input has been completed.
        /// </summary>
        /// <param name="socketError">This indicates the status of the input operation - whether Success or some error.</param>
        /// <param name="bytesTransferred">the number of bytes that were transferred</param>
        /// <exception cref="NetMQException">A non-recoverable socket-error occurred.</exception>
        public void InCompleted(SocketError socketError, int bytesTransferred)
        {
           

            switch (socketError)
            {
                case SocketError.Success:
                {
                    // TODO: check TcpFilters
                   

                    if (m_options.TcpKeepalive != -1)
                    {
                      

                        if (m_options.TcpKeepaliveIdle != -1 && m_options.TcpKeepaliveIntvl != -1)
                        {
                            var bytes = new ByteArraySegment(new byte[12]);

                            Endianness endian = BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;

                            bytes.PutInteger(endian, m_options.TcpKeepalive, 0);
                            bytes.PutInteger(endian, m_options.TcpKeepaliveIdle, 4);
                            bytes.PutInteger(endian, m_options.TcpKeepaliveIntvl, 8);

                           
                        }
                    }

                    // Create the engine object for this connection.
                  
                    // Choose I/O thread to run connector in. Given that we are already
                    // running in an I/O thread, there must be at least one available.
                    IOThread? ioThread = ChooseIOThread(m_options.Affinity);

                  

                    // Create and launch a session object.
                    // TODO: send null in address parameter, is unneeded in this case
                   
                

                    
                    Accept();
                    break;
                }
                case SocketError.ConnectionReset:
                case SocketError.NoBufferSpaceAvailable:
                case SocketError.TooManyOpenSockets:
                {
                    m_socket.EventAcceptFailed(m_endpoint, socketError.ToErrorCode());

                    Accept();
                    break;
                }
                default:
                {
                    NetMQException exception = NetMQException.Create(socketError);

                    m_socket.EventAcceptFailed(m_endpoint, exception.ErrorCode);
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Close the listening socket.
        /// </summary>
        private void Close()
        {
            if (m_handle == null)
                return;

         

            try
            {
            
            }
            catch (SocketException ex)
            {
                m_socket.EventCloseFailed(m_endpoint, ex.SocketErrorCode.ToErrorCode());
            }

            m_handle = null;
        }

        /// <summary>
        /// Get the bound address for use with wildcards
        /// </summary>
        public virtual string Address => m_address.ToString();

        /// <summary>
        /// Get the port-number to listen on.
        /// </summary>
        public virtual int Port => m_port;

        /// <summary>
        /// This method would be called when a message Send operation has been completed - except in this case it simply throws a NotImplementedException.
        /// </summary>
        /// <param name="socketError">a SocketError value that indicates whether Success or an error occurred</param>
        /// <param name="bytesTransferred">the number of bytes that were transferred</param>
        /// <exception cref="NotImplementedException">OutCompleted is not implemented on TcpListener.</exception>
        void IProactorEvents.OutCompleted(SocketError socketError, int bytesTransferred)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This would be called when a timer expires, although here it only throws a NotSupportedException.
        /// </summary>
        /// <param name="id">an integer used to identify the timer (not used here)</param>
        /// <exception cref="NotSupportedException">TimerEvent is not supported on TcpListener.</exception>
        public void TimerEvent(int id)
        {
            throw new NotSupportedException();
        }
    }
}
