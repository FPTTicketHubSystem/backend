using System.Runtime.CompilerServices;
using System.Web;

namespace backend.Helper
{
    public class EmailHelper
    {
        private static EmailHelper instance;
        public static EmailHelper Instance
        {
            get { if (instance == null) instance = new EmailHelper(); return EmailHelper.instance; }
            private set { EmailHelper.instance = value; }
        }

        public DateTime ConvertUtcToVietnamTime(DateTime utcTime)
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, vietnamTimeZone);

            return vietnamTime;
        }


        public string RegisterMail(string fullname, string? emailEncypt, string email)
        {
            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""utf-8"">
                        <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
                        <title>Email Confirmation</title>
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                        <style type=""text/css"">
                            @media screen {{
                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 400;
                                    src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                                }}

                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 700;
                                    src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                                }}
                            }}

                            body,
                            table,
                            td,
                            a {{
                                -ms-text-size-adjust: 100%;
                                -webkit-text-size-adjust: 100%;
                            }}

                            table,
                            td {{
                                mso-table-rspace: 0pt;
                                mso-table-lspace: 0pt;
                            }}

                            img {{
                                -ms-interpolation-mode: bicubic;
                            }}

                            a[x-apple-data-detectors] {{
                                font-family: inherit !important;
                                font-size: inherit !important;
                                font-weight: inherit !important;
                                line-height: inherit !important;
                                color: inherit !important;
                                text-decoration: none !important;
                            }}

                            div[style*=""margin: 16px 0;""] {{
                                margin: 0 !important;
                            }}

                            body {{
                                width: 100% !important;
                                height: 100% !important;
                                padding: 0 !important;
                                margin: 0 !important;
                                background-color: #e9ecef;
                            }}

                            table {{
                                border-collapse: collapse !important;
                            }}

                            img {{
                                height: auto;
                                line-height: 100%;
                                text-decoration: none;
                                border: 0;
                                outline: none;
                            }}

                            .button {{
                                display: inline-block;
                                padding: 12px 30px;
                                font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                                font-size: 16px;
                                text-decoration: none;
                                border-radius: 50px;
                                background-color: #EC6C21;
                                color: #ffffff !important;
                                margin-top: 20px;
                            }}

                            .button:hover {{
                                background-color: #81360b;
                            }}
                        </style>

                    </head>

                    <body>
                        <div class=""preheader"" style=""display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;"">
                        </div>
                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr style=""height: full;"">
                                            <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                                <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px;"">
                                                <p style=""margin: 0;"">Dear {fullname}, <br />
                                                    Chúng tôi nhận được yêu cầu đăng ký của bạn với email {email}<br /><br />
                                                    Bấm nút xác thực bên dưới để xác nhận tài khoản.<br />
                                                    <a href=""https://frontend-nine-brown-60.vercel.app/confirmaccount/{emailEncypt}"" target=""_blank"" class=""button"">Xác thực</a><br /><br />
                                                    Chúc bạn có trải nghiệm tốt.
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"">
                                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                    <tr>
                                                        <td align=""center"" bgcolor=""#ffffff"" style=""padding: 12px;"">
                                                            <table border=""0"" cellpadding=""0"" cellspacing=""0"">
                                                                <tr>
                                                                    <td align=""center"" bgcolor=""#1a82e2"" style=""border-radius: 6px;"">

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf"">
                                                <p style=""margin: 0;""><br></p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";
        }

        public string ForgotMail(string fullname, string? email, string password)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta http-equiv='x-ua-compatible' content='ie=edge'>
                    <title>Forgot Password</title>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <style type='text/css'>
                        @media screen {{
                            @font-face {{
                                font-family: 'Source Sans Pro';
                                font-style: normal;
                                font-weight: 400;
                                src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                            }}

                            @font-face {{
                                font-family: 'Source Sans Pro';
                                font-style: normal;
                                font-weight: 700;
                                src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                            }}
                        }}

                        body,
                        table,
                        td,
                        a {{
                            -ms-text-size-adjust: 100%;
                            -webkit-text-size-adjust: 100%;
                        }}

                        table,
                        td {{
                            mso-table-rspace: 0pt;
                            mso-table-lspace: 0pt;
                        }}

                        img {{
                            -ms-interpolation-mode: bicubic;
                        }}

                        a[x-apple-data-detectors] {{
                            font-family: inherit !important;
                            font-size: inherit !important;
                            font-weight: inherit !important;
                            line-height: inherit !important;
                            color: inherit !important;
                            text-decoration: none !important;
                        }}

                        div[style*='margin: 16px 0;'] {{
                            margin: 0 !important;
                        }}

                        body {{
                            width: 100% !important;
                            height: 100% !important;
                            padding: 0 !important;
                            margin: 0 !important;
                            font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                        }}

                        table {{
                            border-collapse: collapse !important;
                        }}

                        a {{
                            color: #1a82e2;
                        }}

                        img {{
                            height: auto;
                            line-height: 100%;
                            text-decoration: none;
                            border: 0;
                            outline: none;
                        }}

                        .button {{
                            display: inline-block;
                            font-size: 16px;
                            color: #ffffff;
                            text-decoration: none;
                            background-color: #1a82e2;
                            padding: 10px 20px;
                            border-radius: 5px;
                            border: none;
                            cursor: pointer;
                            transition: background-color 0.3s ease;
                        }}

                        .button:hover {{
                            background-color: #0056b3;
                        }}

                        .preheader {{
                            display: none;
                            max-width: 0;
                            max-height: 0;
                            overflow: hidden;
                            font-size: 1px;
                            line-height: 1px;
                            color: #fff;
                            opacity: 0;
                        }}
                    </style>
                </head>

                <body style='background-color: #e9ecef;'>
                    <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                        <tr>
                            <td align='center' bgcolor='#e9ecef'>
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                                    <tr style=""height: full;"">
                                        <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                            <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align='center' bgcolor='#e9ecef'>
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                                    <tr>
                                        <td align='left' bgcolor='#ffffff' style='padding: 24px; font-size: 16px; line-height: 24px;'>
                                            <p style='margin: 0;'>Dear {fullname}, <br />
                                                Chúng tôi đã nhận được yêu cầu mật khẩu mới từ tài khoản {email} của bạn. Hãy sử dụng mật khẩu dưới đây để đăng nhập:
                                                <br/><br/>
                                                Mật khẩu mới: <strong>{password}</strong><br /><br />
                                                Không chia sẻ mật khẩu này với bất kỳ ai. Bạn nên đổi mật khẩu sau khi nhận được mật khẩu này.
                                                <br/><br/>
                                                Chúc bạn có trải nghiệm tốt!
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left' bgcolor='#ffffff' style='padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf;'>
                                            <p style='margin: 0;'><br></p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
        }

        public string EventReminderMail(string fullname, string eventName, DateTime eventStartTime, string eventLocation, string eventAddress)
        {
            string eventLocationEncoded = HttpUtility.UrlEncode(eventLocation);
            string eventAddressEncoded = HttpUtility.UrlEncode(eventAddress);
            DateTime eventStartTimeLocal = ConvertUtcToVietnamTime(eventStartTime);
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
                    <title>EventReminder</title>
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                    <style type=""text/css"">
                        @media screen {{
                            @font-face {{
                                font-family: 'Source Sans Pro';
                                font-style: normal;
                                font-weight: 400;
                                src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                            }}

                            @font-face {{
                                font-family: 'Source Sans Pro';
                                font-style: normal;
                                font-weight: 700;
                                src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                            }}
                        }}

                        body,
                        table,
                        td,
                        a {{
                            -ms-text-size-adjust: 100%;
                            -webkit-text-size-adjust: 100%;
                        }}

                        table,
                        td {{
                            mso-table-rspace: 0pt;
                            mso-table-lspace: 0pt;
                        }}

                        body 

                        img {{
                            -ms-interpolation-mode: bicubic;
                        }}

                        a[x-apple-data-detectors] {{
                            font-family: inherit !important;
                            font-size: inherit !important;
                            font-weight: inherit !important;
                            line-height: inherit !important;
                            color: inherit !important;
                            text-decoration: none !important;
                        }}

                        div[style*=""margin: 16px 0;""] {{
                            margin: 0 !important;
                        }}

                        body {{
                            width: 100% !important;
                            height: 100% !important;
                            padding: 0 !important;
                            margin: 0 !important;
                            font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                        }}

                        table {{
                            border-collapse: collapse !important;
                        }}

                        a {{
                            color: #1a82e2;
                        }}

                        img {{
                            height: auto;
                            line-height: 100%;
                            text-decoration: none;
                            border: 0;
                            outline: none;
                        }}

                        .button {{
                            display: inline-block;
                            font-size: 16px;
                            color: #ffffff;
                            text-decoration: none;
                            background-color: #1a82e2;
                            padding: 10px 20px;
                            border-radius: 5px;
                            border: none;
                            cursor: pointer;
                            transition: background-color 0.3s ease;
                        }}

                        .button:hover {{
                            background-color: #0056b3;
                        }}

                        .preheader {{
                            display: none;
                            max-width: 0;
                            max-height: 0;
                            overflow: hidden;
                            font-size: 1px;
                            line-height: 1px;
                            color: #fff;
                            opacity: 0;
                        }}
                    </style>
                </head>

                <body style=""background-color: #e9ecef;"">
                    <div class=""preheader"">
                        EventReminder
                    </div>
                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                        <tr>
                            <td align=""center"" bgcolor=""#e9ecef"">
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                    <tr style=""height: full;"">
                                        <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                            <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align=""center"" bgcolor=""#e9ecef"">
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                    <tr>
                                        <td align='left' bgcolor='#ffffff' style='padding: 24px; font-size: 16px; line-height: 24px;'>
                                            <p style='margin: 0;'>Dear {fullname},<br /><br />
                                                Sự kiện <strong>{eventName}</strong> mà bạn đã nhận vé tham gia sẽ diễn ra vào ngày mai.
                                                <br/><br/>
                                                <strong>Thời gian bắt đầu:</strong> {eventStartTimeLocal:dd/MM/yyyy HH:mm}<br/>
                                                <strong>Tên địa điểm:</strong> {eventLocation}<br/>
                                                <strong>Địa chỉ:</strong> {eventAddress}<br/>
                                                <a href=""https://www.google.com/maps/search/{eventLocationEncoded},%20{eventAddressEncoded}"" target=""_blank"">Xem trên Google Maps</a><br/><br/>
                                                Chúng tôi mong chờ sự có mặt của bạn tại sự kiện. Chúc bạn có trải nghiệm tốt!
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf;"">
                                            <p style=""margin: 0;""><br></p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
        }
        public string RatingRequestMail(string fullname, string eventName, int eventRatingId)
        {
            string rateEventUrl = $"https://frontend-nine-brown-60.vercel.app/rate/{eventRatingId}";

            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"">
                    <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
                    <title>EventRatingRequest</title>
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                    <style type=""text/css"">
                        @media screen {{
                            @font-face {{
                                font-family: 'Source Sans Pro';
                                font-style: normal;
                                font-weight: 400;
                                src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                            }}

                            @font-face {{
                                font-family: 'Source Sans Pro';
                                font-style: normal;
                                font-weight: 700;
                                src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                            }}
                        }}

                        body,
                        table,
                        td,
                        a {{
                            -ms-text-size-adjust: 100%;
                            -webkit-text-size-adjust: 100%;
                        }}

                        table,
                        td {{
                            mso-table-rspace: 0pt;
                            mso-table-lspace: 0pt;
                        }}

                        body,
                        img {{
                            -ms-interpolation-mode: bicubic;
                        }}

                        a[x-apple-data-detectors] {{
                            font-family: inherit !important;
                            font-size: inherit !important;
                            font-weight: inherit !important;
                            line-height: inherit !important;
                            color: inherit !important;
                            text-decoration: none !important;
                        }}

                        div[style*=""margin: 16px 0;""] {{
                            margin: 0 !important;
                        }}

                        body {{
                            width: 100% !important;
                            height: 100% !important;
                            padding: 0 !important;
                            margin: 0 !important;
                            font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                        }}

                        table {{
                            border-collapse: collapse !important;
                        }}

                        a {{
                            color: #1a82e2;
                        }}

                        img {{
                            height: auto;
                            line-height: 100%;
                            text-decoration: none;
                            border: 0;
                            outline: none;
                        }}

                        .button {{
                                display: inline-block;
                                padding: 12px 30px;
                                font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                                font-size: 16px;
                                text-decoration: none;
                                border-radius: 50px;
                                background-color: #EC6C21;
                                color: #ffffff !important;
                                margin-top: 20px;
                            }}

                        .button:hover {{
                                background-color: #81360b;
                            }}

                        .preheader {{
                            display: none;
                            max-width: 0;
                            max-height: 0;
                            overflow: hidden;
                            font-size: 1px;
                            line-height: 1px;
                            color: #fff;
                            opacity: 0;
                        }}
                    </style>
                </head>

                <body style=""background-color: #e9ecef;"">
                    <div class=""preheader"">
                        EventRatingRequest
                    </div>
                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                        <tr>
                            <td align=""center"" bgcolor=""#e9ecef"">
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                    <tr style=""height: full;"">
                                        <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                            <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align=""center"" bgcolor=""#e9ecef"">
                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                    <tr>
                                        <td align='left' bgcolor='#ffffff' style='padding: 24px; font-size: 16px; line-height: 24px;'>
                                            <p style='margin: 0;'>Xin chào {fullname},<br /><br />
                                                Cảm ơn bạn đã tham gia sự kiện <strong>{eventName}</strong>. Chúng tôi rất mong nhận được đánh giá của bạn về sự kiện này.
                                                <br/><br/>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf;"">
                                            <p style=""margin: 0;"">
                                                <a href=""{rateEventUrl}"" class=""button"" target=""_blank"">Đánh giá sự kiện</a>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
        }

        public string TicketEmail(string fullName, int ticketId, string ticketType, int quantity, string orderId, string paymentAmount, string eventName, DateTime eventStartTime, string eventLocation, string eventAddress)
        {
            string eventLocationEncoded = HttpUtility.UrlEncode(eventLocation);
            string eventAddressEncoded = HttpUtility.UrlEncode(eventAddress);
            DateTime eventStartTimeLocal = ConvertUtcToVietnamTime(eventStartTime);
            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""utf-8"">
                        <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
                        <title>Your Ticket</title>
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                        <style type=""text/css"">
                            @media screen {{
                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 400;
                                    src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                                }}

                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 700;
                                    src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                                }}
                            }}

                            body,
                            table,
                            td,
                            a {{
                                -ms-text-size-adjust: 100%;
                                -webkit-text-size-adjust: 100%;
                            }}

                            table,
                            td {{
                                mso-table-rspace: 0pt;
                                mso-table-lspace: 0pt;
                            }}

                            img {{
                                -ms-interpolation-mode: bicubic;
                            }}

                            a[x-apple-data-detectors] {{
                                font-family: inherit !important;
                                font-size: inherit !important;
                                font-weight: inherit !important;
                                line-height: inherit !important;
                                color: inherit !important;
                                text-decoration: none !important;
                            }}

                            div[style*=""margin: 16px 0;""] {{
                                margin: 0 !important;
                            }}

                            body {{
                                width: 100% !important;
                                height: 100% !important;
                                padding: 0 !important;
                                margin: 0 !important;
                                font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                            }}

                            table {{
                                border-collapse: collapse !important;
                            }}

                            a {{
                                color: #1a82e2;
                            }}

                            img {{
                                height: auto;
                                line-height: 100%;
                                text-decoration: none;
                                border: 0;
                                outline: none;
                            }}

                            .button {{
                                display: inline-block;
                                font-size: 16px;
                                color: #ffffff;
                                text-decoration: none;
                                background-color: #1a82e2;
                                padding: 10px 20px;
                                border-radius: 5px;
                                border: none;
                                cursor: pointer;
                                transition: background-color 0.3s ease;
                            }}

                            .button:hover {{
                                background-color: #0056b3;
                            }}

                            .preheader {{
                                display: none;
                                max-width: 0;
                                max-height: 0;
                                overflow: hidden;
                                font-size: 1px;
                                line-height: 1px;
                                color: #fff;
                                opacity: 0;
                            }}
                        </style>
                    </head>

                    <body style=""background-color: #e9ecef;"">
                        <div class=""preheader"">
                            This is a preheader text.
                        </div>
                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr style=""height: full;"">
                                            <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                                <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px;"">
                                                <p style=""margin: 0;"">Dear {fullName},<br /><br />
                                                    Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi! Dưới đây là vé của bạn dưới dạng QR Code. Vui lòng sử dụng nó để vào sự kiện:
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""center"" bgcolor=""#ffffff"" style=""padding: 24px;"">
                                                <img src=""https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={ticketId}"" alt=""{ticketId}"" style=""border: 0; outline: none; text-decoration: none; display: block; margin: auto;"">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""center"" bgcolor=""#ffffff"">
                                                <p><strong>{ticketId}</strong></p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""center"" bgcolor=""#ffffff"">
                                                <p><strong>Hạng vé: </strong>{ticketType}</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""center"" bgcolor=""#ffffff"">
                                                <p><strong>Số lượng: </strong>{quantity}</p>
                                            </td>
                                        </tr>
                                        <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px;"">
                                            <p style=""margin: 0;"">Thông tin vé: <br /> <br />
                                                <strong>Mã order: </strong> {orderId}<br/>
                                                <strong>Tổng thanh toán: </strong> {paymentAmount}<br/><br/>
                                                <strong>Tên sự kiện: </strong>{eventName}<br/>
                                                    <strong>Thời gian bắt đầu:</strong> {eventStartTimeLocal:dd/MM/yyyy HH:mm}<br/>
                                                    <strong>Tên địa điểm:</strong> {eventLocation}<br/>
                                                    <strong>Địa chỉ: </strong>{eventAddress}<br/>
                                                    <a href=""https://www.google.com/maps/search/{eventLocationEncoded},%20{eventAddressEncoded}"" target=""_blank"">Xem trên Google Maps</a><br/><br/>
                                            </p>
                                        </td>
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf;"">
                                                <p style=""margin: 0;"">Ban tổ chức mong chờ sự tham gia của bạn tại sự kiện. Chúc bạn có trải nghiệm tốt.</p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>";
        }

        public string ApprovedEventFirstTimeEmail(string email, string fullName, string eventName)
        {
            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""utf-8"">
                        <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
                        <title>Event Approved</title>
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                        <style type=""text/css"">
                            @media screen {{
                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 400;
                                    src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                                }}

                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 700;
                                    src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                                }}
                            }}

                            body,
                            table,
                            td,
                            a {{
                                -ms-text-size-adjust: 100%;
                                -webkit-text-size-adjust: 100%;
                            }}

                            table,
                            td {{
                                mso-table-rspace: 0pt;
                                mso-table-lspace: 0pt;
                            }}

                            body 

                            img {{
                                -ms-interpolation-mode: bicubic;
                            }}

                            a[x-apple-data-detectors] {{
                                font-family: inherit !important;
                                font-size: inherit !important;
                                font-weight: inherit !important;
                                line-height: inherit !important;
                                color: inherit !important;
                                text-decoration: none !important;
                            }}

                            div[style*=""margin: 16px 0;""] {{
                                margin: 0 !important;
                            }}

                            body {{
                                width: 100% !important;
                                height: 100% !important;
                                padding: 0 !important;
                                margin: 0 !important;
                                font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                            }}

                            table {{
                                border-collapse: collapse !important;
                            }}

                            a {{
                                color: #1a82e2;
                            }}

                            img {{
                                height: auto;
                                line-height: 100%;
                                text-decoration: none;
                                border: 0;
                                outline: none;
                            }}

                            .button {{
                                display: inline-block;
                                font-size: 16px;
                                color: #ffffff;
                                text-decoration: none;
                                background-color: #1a82e2;
                                padding: 10px 20px;
                                border-radius: 5px;
                                border: none;
                                cursor: pointer;
                                transition: background-color 0.3s ease;
                            }}

                            .button:hover {{
                                background-color: #0056b3;
                            }}

                            .preheader {{
                                display: none;
                                max-width: 0;
                                max-height: 0;
                                overflow: hidden;
                                font-size: 1px;
                                line-height: 1px;
                                color: #fff;
                                opacity: 0;
                            }}
                        </style>
                    </head>

                    <body style=""background-color: #e9ecef;"">
                        <div class=""preheader"">
                            Event Approved
                        </div>
                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr style=""height: full;"">
                                            <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                                <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr>
                                            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-size: 16px; line-height: 24px;'>
                                                <p style='margin: 0;'>Dear {fullName},<br /> <br/>
                                                    Bạn đã trở thành nhà tổ chức sự kiện. <br /> <br/>
                                                    Sự kiện <strong>{eventName}</strong> của bạn đã được phê duyệt  .
                                                    <br/><br/>
                                                    Lưu ý cập nhật lại hồ sơ ban tổ chức của bạn!<br/>
                                                    Các nội dung cần cập nhật: Tên ban tổ chức, số điện thoại liên lạc, ngày thành lập. <br/> <br/>
                                                    Đội ngũ FPTTicketHub chúc sự kiện của bạn thành công tốt đẹp! <br/> <br/>
                                                    Chân thành cảm ơn!
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf;"">
                                                <p style=""margin: 0;""><br></p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>
            ";
        }

        public string ApprovedEventEmail(string email, string fullName, string eventName)
        {
            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""utf-8"">
                        <meta http-equiv=""x-ua-compatible"" content=""ie=edge"">
                        <title>Event Approved</title>
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                        <style type=""text/css"">
                            @media screen {{
                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 400;
                                    src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff');
                                }}

                                @font-face {{
                                    font-family: 'Source Sans Pro';
                                    font-style: normal;
                                    font-weight: 700;
                                    src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff');
                                }}
                            }}

                            body,
                            table,
                            td,
                            a {{
                                -ms-text-size-adjust: 100%;
                                -webkit-text-size-adjust: 100%;
                            }}

                            table,
                            td {{
                                mso-table-rspace: 0pt;
                                mso-table-lspace: 0pt;
                            }}

                            body 

                            img {{
                                -ms-interpolation-mode: bicubic;
                            }}

                            a[x-apple-data-detectors] {{
                                font-family: inherit !important;
                                font-size: inherit !important;
                                font-weight: inherit !important;
                                line-height: inherit !important;
                                color: inherit !important;
                                text-decoration: none !important;
                            }}

                            div[style*=""margin: 16px 0;""] {{
                                margin: 0 !important;
                            }}

                            body {{
                                width: 100% !important;
                                height: 100% !important;
                                padding: 0 !important;
                                margin: 0 !important;
                                font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif;
                            }}

                            table {{
                                border-collapse: collapse !important;
                            }}

                            a {{
                                color: #1a82e2;
                            }}

                            img {{
                                height: auto;
                                line-height: 100%;
                                text-decoration: none;
                                border: 0;
                                outline: none;
                            }}

                            .button {{
                                display: inline-block;
                                font-size: 16px;
                                color: #ffffff;
                                text-decoration: none;
                                background-color: #1a82e2;
                                padding: 10px 20px;
                                border-radius: 5px;
                                border: none;
                                cursor: pointer;
                                transition: background-color 0.3s ease;
                            }}

                            .button:hover {{
                                background-color: #0056b3;
                            }}

                            .preheader {{
                                display: none;
                                max-width: 0;
                                max-height: 0;
                                overflow: hidden;
                                font-size: 1px;
                                line-height: 1px;
                                color: #fff;
                                opacity: 0;
                            }}
                        </style>
                    </head>

                    <body style=""background-color: #e9ecef;"">
                        <div class=""preheader"">
                            Event Approved
                        </div>
                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr style=""height: full;"">
                                            <td align=""left"" bgcolor=""#150d0a"" style=""border-bottom: solid 5px #EC6C21; text-align: center;"">
                                                <img src=""https://i.imgur.com/jxQVXhx.png"" alt=""logo"" style=""width: full; height: 100px;"">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align=""center"" bgcolor=""#e9ecef"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px;"">
                                        <tr>
                                            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-size: 16px; line-height: 24px;'>
                                                <p style='margin: 0;'>Dear {fullName},<br /> <br/>
                                                    Sự kiện <strong>{eventName}</strong> của bạn đã được phê duyệt.
                                                    <br/><br/>
                                                    Đội ngũ FPTTicketHub chúc sự kiện của bạn thành công tốt đẹp! <br/> <br/>
                                                    Chân thành cảm ơn!
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align=""left"" bgcolor=""#ffffff"" style=""padding: 24px; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf;"">
                                                <p style=""margin: 0;""><br></p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </body>
                    </html>
            ";
        }

    }
}
    