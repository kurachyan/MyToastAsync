using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ToastAsync
{
    public class CS_ToastAsync
    {
        #region 共有領域
        private String _Title;
        private String _Message;
        private int _Toast_Type;
        private Boolean _Show_Time;
        private int _ToastTemplate;
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }
        public String Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
            }
        }
        public int Toast_Type
        {
            get
            {
                return _Toast_Type;
            }
            set
            {
                _Toast_Type = value;
            }
        }
        public Boolean Show_Time
        {
            get
            {
                return _Show_Time;
            }
            set
            {
                _Show_Time = value;
            }
        }
        public int Toast_Template
        {
            get
            {
                return _ToastTemplate;
            }
            set
            {
                _ToastTemplate = value;
            }
        }
        private String[] Toast_Table =
        {   // 名称
            "ToastImageAndText01",      // 0
            "ToastImageAndText02",      // 1
            "ToastImageAndText03",      // 2
            "ToastImageAndText04",      // 3
            "ToastText01",              // 4
            "ToastText02",              // 5
            "ToastText03",              // 6
            "ToastText04"               // 7
        };
        private Windows.UI.Notifications.ToastTemplateType _TType;
        public Windows.UI.Notifications.ToastTemplateType TType
        {
            get
            {
                return _TType;
            }
            set
            {
                _TType = value;
            }
        }
        private String[] Sound_Name =
        {   // 鳴音種別
            "ms-winsoundevent:Notification.Default",            // Default
            "ms-winsoundevent:Notification.IM",                 // Instant_Message
            "ms-winsoundevent:Notification.Mail",               // Mail
            "ms-winsoundevent:Notification.Reminder",           // Scadule
            "ms-winsoundevent:Notification.SMS",                // Text_Message
            "ms-winsoundevent:Notification.Looping.Alarm",      // Alarm1
            "ms-winsoundevent:Notification.Looping.Alarm2",     // Alarm2
            "ms-winsoundevent:Notification.Looping.Call",       // Call1
            "ms-winsoundevent:Notification.Looping.Call2"       // Call2
        };
        #endregion

        #region コンストラクタ
        public CS_ToastAsync()
        {
            _Title = "";
            _Message = "";
            _Toast_Type = 0;         // ToastText01
            _Show_Time = false;      // ７秒描画
        }
        public CS_ToastAsync(String __Message)
        {
            if (__Message != "")
            {
                _Message = __Message;
            }
            _Toast_Type = 0;        // ToastText01
            _Show_Time = false;     // ７秒描画
        }
        public CS_ToastAsync(String __Title, String __Message)
        {
            if (__Title != "")
            {
                _Title = __Title;
            }
            if (__Message != "")
            {
                Message = __Message;
            }
            _Toast_Type = 1;        // ToastText02
            _Show_Time = false;     // ７秒描画
        }
        #endregion

        #region 基本設定
        public async Task Set_TitleAsync(String __Title)
        {
            if (__Title != "")
            {
                _Title = __Title;
            }
        }
        public async Task Set_MessageAsync(String __Message)
        {
            //			if(_Toast_Type ==  1 || _Toast_Type == 3)
            //			{
            if (__Message != "")
            {
                _Message = __Message;
            }
            //			}
        }
        public async Task Set_TypeAsync(int __Toast_Type)
        {
            //            if (__Toast_Type < 4)
            //            {
            _Toast_Type = __Toast_Type;
            //            }
        }
        public async Task Set_Show_TimeAsync(Boolean __Show_Time)
        {
            _Show_Time = __Show_Time;
        }
        #endregion

        #region 実行
        public async Task<bool> ExecAsync()
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(_TType);

            switch (_TType)
            {
                case ToastTemplateType.ToastImageAndText01:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastImageAndText02:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastImageAndText03:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastImageAndText04:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText01:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText02:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText03:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText04:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
            }

            var toastElement = (XmlElement)toastXml.GetElementsByTagName("toast")[0];
            await Set_DurationAsync(toastElement);

            XmlElement audio = toastXml.CreateElement("audio");
            //            audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");
            //            audio.SetAttribute("src", Sound_Name[1]);
            //            audio.SetAttribute("loop", "false");		// 繰り返し　：　無し
            await Set_AudioAsync(1, audio);                // Notification.IM（繰り返し：無し）

            toastElement.AppendChild(audio);

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotifier.Show(toast);

            return true;
        }
        public async Task<bool> ExecAsync(DateTimeOffset deliveryTime)
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(_TType);

            switch (_TType)
            {
                case ToastTemplateType.ToastImageAndText01:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastImageAndText02:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastImageAndText03:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastImageAndText04:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText01:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText02:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText03:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
                case ToastTemplateType.ToastText04:
                    toastXml.GetElementsByTagName("text")[0].InnerText = _Title;
                    toastXml.GetElementsByTagName("text")[1].InnerText = _Message;
                    break;
            }

            var toastElement = (XmlElement)toastXml.GetElementsByTagName("toast")[0];
            await Set_DurationAsync(toastElement);

            XmlElement audio = toastXml.CreateElement("audio");
            //            audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");
            //            audio.SetAttribute("src", Sound_Name[1]);
            //            audio.SetAttribute("loop", "false");		// 繰り返し　：　無し
            await Set_AudioAsync(1, audio);                // Notification.IM（繰り返し：無し）

            toastElement.AppendChild(audio);

            var toast = new ScheduledToastNotification(toastXml, deliveryTime);
            ToastNotifier.AddToSchedule(toast);

            return true;
        }
        private async Task Set_DurationAsync(XmlElement toastElement)
        {
            if (_Show_Time == true)
            {
                toastElement.SetAttribute("duration", "long");		// ２５秒表示

            }
            else
            {
                toastElement.SetAttribute("duration", "short");		// ７秒表示

            }

        }
        private async Task Set_AudioAsync(int _Type, XmlElement toastElement)
        {
            toastElement.SetAttribute("src", Sound_Name[_Type]);    // 鳴音種別

            if (_Type > 4)
            {   // 繰り返し有り？
                toastElement.SetAttribute("loop", "true");         // 繰り返し　：　有り
            }
            else
            {
                toastElement.SetAttribute("loop", "false");         // 繰り返し　：　無し
            }
        }
        #endregion

        #region その他
        #endregion
    }
}
