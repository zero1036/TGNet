﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace EG.WeChat.Web.Models
{
    public class ValidateCode
    {
        public static string m_SessionIdentify = "strIdentify";
        /// <summary>
        /// 產生圖形驗證碼。
        /// </summary>
        /// <param name="Code">傳出驗證碼。</param>
        /// <param name="CodeLength">驗證碼字元數。</param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="FontSize"></param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(out String Code, int CodeLength, int Width, int Height, int FontSize)
        {
            String sCode = String.Empty;
            //顏色列表，用於驗證碼、噪線、噪點
            Color[] oColors ={ 
             System.Drawing.Color.Black,
             System.Drawing.Color.Red,
             System.Drawing.Color.Blue,
             System.Drawing.Color.Green,
             System.Drawing.Color.Orange,
             System.Drawing.Color.Brown,
             System.Drawing.Color.Brown,
             System.Drawing.Color.DarkBlue
            };
            //字體列表，用於驗證碼
            string[] oFontNames = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //驗證碼的字元集，去掉了一些容易混淆的字元
            char[] oCharacter = {
       '2','3','4','5','6','8','9',
       'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y'
      };
            Random oRnd = new Random();
            Bitmap oBmp = null;
            Graphics oGraphics = null;
            int N1 = 0;
            System.Drawing.Point oPoint1 = default(System.Drawing.Point);
            System.Drawing.Point oPoint2 = default(System.Drawing.Point);
            string sFontName = null;
            Font oFont = null;
            Color oColor = default(Color);

            //生成驗證碼字串
            for (N1 = 0; N1 <= CodeLength - 1; N1++)
            {
                sCode += oCharacter[oRnd.Next(oCharacter.Length)];
            }

            oBmp = new Bitmap(Width, Height);
            oGraphics = Graphics.FromImage(oBmp);
            oGraphics.Clear(System.Drawing.Color.White);
            try
            {
                for (N1 = 0; N1 <= 4; N1++)
                {
                    //畫噪線
                    oPoint1.X = oRnd.Next(Width);
                    oPoint1.Y = oRnd.Next(Height);
                    oPoint2.X = oRnd.Next(Width);
                    oPoint2.Y = oRnd.Next(Height);
                    oColor = oColors[oRnd.Next(oColors.Length)];
                    oGraphics.DrawLine(new Pen(oColor), oPoint1, oPoint2);
                }

                float spaceWith = 0, dotX = 0, dotY = 0;
                if (CodeLength != 0)
                {
                    spaceWith = (Width - FontSize * CodeLength - 10) / CodeLength;
                }

                for (N1 = 0; N1 <= sCode.Length - 1; N1++)
                {
                    //畫驗證碼字串
                    sFontName = oFontNames[oRnd.Next(oFontNames.Length)];
                    oFont = new Font(sFontName, FontSize, FontStyle.Italic);
                    oColor = oColors[oRnd.Next(oColors.Length)];

                    dotY = (Height - oFont.Height) / 2 + 2;//中心下移2像素
                    dotX = Convert.ToSingle(N1) * FontSize + (N1 + 1) * spaceWith;

                    oGraphics.DrawString(sCode[N1].ToString(), oFont, new SolidBrush(oColor), dotX, dotY);
                }

                for (int i = 0; i <= 30; i++)
                {
                    //畫噪點
                    int x = oRnd.Next(oBmp.Width);
                    int y = oRnd.Next(oBmp.Height);
                    Color clr = oColors[oRnd.Next(oColors.Length)];
                    oBmp.SetPixel(x, y, clr);
                }

                Code = sCode;
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                oBmp.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                oGraphics.Dispose();
            }
        }
        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="pCurCode"></param>
        /// <returns></returns>
        public static string ValidateCodeByRequest(string pCurCode)
        {
            string resultMsg = string.Empty;

            object objTargetCode = SessionHelper.Get(m_SessionIdentify);
            if (objTargetCode == null)
            {
                resultMsg = "请输入验证码";
            }
            string pTargetCode = objTargetCode.ToString();
            if (string.IsNullOrEmpty(pTargetCode))
            {
                resultMsg = "验证码过期";
            }
            else if (pTargetCode.ToLower() != pCurCode.ToLower())
            {
                resultMsg = "验证码不正确";
            }
            return resultMsg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic()
        {
            int width = 60;// ConverterHelper.ObjToInt(Request.Params["width"], 100);
            int height = 25;// ConverterHelper.ObjToInt(Request.Params["height"], 40);
            int fontsize = 20;// ConverterHelper.ObjToInt(Request.Params["fontsize"], 20);

            string code = string.Empty;
            byte[] bytes = CreateValidateGraphic(out code, 4, width, height, fontsize);

            //string strTime = Request.Params["t"].ToString();
            SessionHelper.Add(m_SessionIdentify, code);
            return bytes;
        }
    }

  
}