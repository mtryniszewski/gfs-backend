using GFS.Core.Enums;

namespace GFS.Core
{
    public class Dictionary
    {
        public string UserNotFound { get; set; }
        public string WrongPassword { get; set; }
        public string AccountNotActive { get; set; }
        public string UserRegistrationFailed { get; set; }
        public string EmptyPassword { get; set; }
        public string ChangePasswordFailed { get; set; }
        public string EmptyToken { get; set; }
        public string OrderNotFound { get; set; }
        public string WrongFurnitureType { get; set; }
        public string WrongFrontCount { get; set; }
        public string FabricNotFound { get; set; }
        public string ProducerNotFound { get; set; }
        public string WrongDrawerType { get; set; }
        public string WrongDrawerConfiguration { get; set; }
        public string WrongActivationToken { get; set; }
        public string UserExists { get; set; }
        public string WrongEmail { get; set; }
        public string UnexpectedError { get; set; }

        public string ActivationMailSubject { get; set; }
        public string ActivationMailMessage { get; set; }
        public string ResetMailSubject { get; set; }
        public string ResetMailMessage { get; set; }

        public string LengthMilling { get; set; }
        public string WidthMilling { get; set; }
        public string DefaultMilling { get; set; }

        public string SingleFormatter { get; set; }
        public string SinkBottom { get; set; }
        public string BlindSideBottom { get; set; }
        public string PentagonCornerBottom { get; set; }
        public string CutFinalBottom { get; set; }
        public string BasicBottom { get; set; }
        public string TwoPartsHigh { get; set; }
        public string BasicWidthDrawerBottom { get; set; }
        public string OvenMicroHigh { get; set; }
        public string LCornerBottom { get; set; }
        public string ThreePartsHigh { get; set; }
        public string OnlyDrawersBottom { get; set; }
        public string BasicTop { get; set; }
        public string OneHorizontalTop { get; set; }
        public string TwoHorizontalTop { get; set; }
        public string ThreeHorizontalTop { get; set; }
        public string PentagonCornerTop { get; set; }
        public string BasicGlassTop { get; set; }
        public string OneHorizontalGlassTop { get; set; }
        public string TwoHorizontalGlassTop { get; set; }
        public string ThreeHorizontalGlassTop { get; set; }
        public string LCornerTop { get; set; }


        public string GetMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.UserNotFound:
                    return UserNotFound;
                case ErrorCode.WrongPassword:
                    return WrongPassword;
                case ErrorCode.AccountNotActive:
                    return AccountNotActive;
                case ErrorCode.UserRegistrationFailed:
                    return UserRegistrationFailed;
                case ErrorCode.EmptyPassword:
                    return EmptyPassword;
                case ErrorCode.ChangePasswordFailed:
                    return ChangePasswordFailed;
                case ErrorCode.EmptyToken:
                    return EmptyToken;
                case ErrorCode.OrderNotFound:
                    return OrderNotFound;
                case ErrorCode.WrongFurnitureType:
                    return WrongFurnitureType;
                case ErrorCode.WrongFrontCount:
                    return WrongFrontCount;
                case ErrorCode.FabricNotFound:
                    return FabricNotFound;
                case ErrorCode.ProducerNotFound:
                    return ProducerNotFound;
                case ErrorCode.WrongDrawerType:
                    return WrongDrawerType;
                case ErrorCode.WrongDrawerConfiguration:
                    return WrongDrawerConfiguration;
                case ErrorCode.WrongActivationToken:
                    return WrongActivationToken;
                case ErrorCode.UserExists:
                    return UserExists;
                case ErrorCode.WrongEmail:
                    return WrongEmail;
                default:
                    return UnexpectedError;
            }
        }

        public string GetMillingType(Milling? milling)
        {
            if (!milling.HasValue)
            {
                return DefaultMilling;
            }
            switch (milling.Value)
            {
                case Milling.LengthMilling:
                    return LengthMilling;
                case Milling.WidthMilling:
                    return WidthMilling;
                default:
                    return DefaultMilling;
            }
        }

        public string GetFurnitureType(FurnitureType furnitureType)
        {
            switch (furnitureType)
            {
                case FurnitureType.SingleFormatter:
                    return SingleFormatter;
                case FurnitureType.SinkBottom:
                    return SinkBottom;
                case FurnitureType.BlindSideBottom:
                    return BlindSideBottom;
                case FurnitureType.PentagonCornerBottom:
                    return PentagonCornerBottom;
                case FurnitureType.CutFinalBottom:
                    return CutFinalBottom;
                case FurnitureType.BasicBottom:
                    return BasicBottom;
                case FurnitureType.TwoPartsHigh:
                    return TwoPartsHigh;
                case FurnitureType.BasicWithDrawerBottom:
                    return BasicWidthDrawerBottom;
                case FurnitureType.OvenMicroHigh:
                    return OvenMicroHigh;
                case FurnitureType.LCornerBottom:
                    return LCornerBottom;
                case FurnitureType.ThreePartsHigh:
                    return ThreePartsHigh;
                case FurnitureType.OnlyDrawersBottom:
                    return OnlyDrawersBottom;
                case FurnitureType.BasicTop:
                    return BasicTop;
                case FurnitureType.OneHorizontalTop:
                    return OneHorizontalTop;
                case FurnitureType.TwoHorizontalTop:
                    return TwoHorizontalTop;
                case FurnitureType.ThreeHorizontalTop:
                    return ThreeHorizontalTop;
                case FurnitureType.PentagonCornerTop:
                    return PentagonCornerTop;
                case FurnitureType.BasicGlassTop:
                    return BasicGlassTop;
                case FurnitureType.OneHorizontalGlassTop:
                    return OneHorizontalGlassTop;
                case FurnitureType.TwoHorizontalGlassTop:
                    return TwoHorizontalGlassTop;
                case FurnitureType.ThreeHorizontalGlassTop:
                    return ThreeHorizontalGlassTop;
                case FurnitureType.LCornerTop:
                    return LCornerTop;
                default:
                    return UnexpectedError;
            }
        }
    }
}