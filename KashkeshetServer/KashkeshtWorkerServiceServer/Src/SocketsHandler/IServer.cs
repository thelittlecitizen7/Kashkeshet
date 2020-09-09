using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.SocketsHandler
{
    public interface IServer
    {
        void Close();

        void Listen();
    }
}
