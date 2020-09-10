using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.CommandOptions
{
    public class GetTimeZoneOption : IOption
    {
        

        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;
        public GetTimeZoneOption(IClientModel userClient, IContainerInterfaces containerInterfaces)
        {
            _userClient = userClient;
            _containerInterfaces = containerInterfaces;

            
        }
        public void Operation(MainRequest chatData)
        {
            RestClient client = new RestClient("http://worldtimeapi.org/api/timezone/Asia/Jerusalem");
            RestRequest request = new RestRequest(Method.GET);
            
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK) 
            {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"The user name {_userClient.Name} cannot add user to group beacuse he has not permission"
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                return;
            }

            var body = Utils.DeSerlizeObject<TimeZoneModel>(response.Content);

            var successBody = new OkResponseMessage
            {
                RequestType = MessageType.SuccessResponse,
                Message = $"time now in Isreal : {body.Datatime} and number of day {body.DayOfWeekNumber}"
            };
            _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(successBody));

            

        }
    }
}
