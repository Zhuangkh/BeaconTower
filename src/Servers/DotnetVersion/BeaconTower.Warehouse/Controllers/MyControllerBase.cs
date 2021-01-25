using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BeaconTower.Warehouse.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        public Response<T> Success<T>(T data, string message = null) 
        {
            Response<T> res = new();
            res.Data = data;
            res.Code = ResponseCode.Success;
            res.Message = message;
            return res;
        }

        public async Task<Response<T>> SuccessAsync<T>(T data, string message = null)
        {
            return await Task.Run(() =>
            {
                Response<T> res = new();
                res.Data = data;
                res.Code = ResponseCode.Success;
                res.Message = message;
                return res;
            });
        }

        public Response<T> Success<T>(T data, int total, string message = null) 
        {
            PageResponse<T> res = new();
            res.Data = data;
            res.Code = ResponseCode.Success;
            res.Total = total;
            res.Message = message;
            return res;
        }

        public async Task<Response<T>> SuccessAsync<T>(T data, int total, string message = null) where T : class
        {
            return await Task.Run(() =>
            {
                PageResponse<T> res = new();
                res.Data = data;
                res.Code = ResponseCode.Success;
                res.Total = total;
                res.Message = message;
                return res;
            });
        }

        public Response<T> Error<T>(ResponseCode code, string message = null) where T : class
        {
            Response<T> res = new();
            res.Code = code;
            res.Message = message;
            return res;
        }

        public async Task<Response<T>> ErrorAsync<T>(ResponseCode code, string message = null) where T : class
        {
            return await Task.Run(() =>
            {
                Response<T> res = new();
                res.Code = code;
                res.Message = message;
                return res;
            });
        }
    }


    [Flags]
    public enum ResponseCode
    {
        Success = 0,
        Error = 1,
        System = 2,
        Logic = 4,
        Parameter = 8,
        Frontend = 16
    }
    public class Response<T> 
    {
        public string Message { get; set; } = string.Empty;
        public ResponseCode Code { get; set; } = ResponseCode.Success;
        public T Data { get; set; }
    }
    public sealed class PageResponse<T> : Response<T>
    {
        public int Total { get; set; }
    }
}
