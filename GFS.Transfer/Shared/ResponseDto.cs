namespace GFS.Transfer.Shared
{
    public class ResponseDto<T>
    {
        public ErrorDto Error { get; set; }

        public T Data { get; set; }

        public static ResponseDto<object> New()
        {
            return new ResponseDto<object>();
        }
    }

    public static class ResponseDto
    {
        public static ResponseDto<object> Default = ResponseDto<object>.New();
    }

    public static class ResponseDtoExtensions
    {
        public static ResponseDto<T> ToResponseDto<T>(this T data)
        {
            return new ResponseDto<T>
            {
                Data = data
            };
        }
    }
}