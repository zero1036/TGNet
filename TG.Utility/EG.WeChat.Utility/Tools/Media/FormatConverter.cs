using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Web;
/*****************************************************
* 目的：視頻相關操作
* 创建人：林子聪
* 创建时间：20150128
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    public class FormatConverter
    {
        #region 私有成員及構造函數
        //FFmpeg配置信息
        private string ffmpegpath = "/Tools/ffmpeg.exe";//FFmpeg的服务器路径
        private string imgsize = "400*300";     //视频截图大小
        private string videosize = "480*360"; //视频大小
        #region 也可将信息添加到配置文件中
        //public static string ffmpegpath = ConfigurationManager.AppSettings["ffmpeg"];
        //public static string imgsize = ConfigurationManager.AppSettings["imgsize"];
        //public static string videosize = ConfigurationManager.AppSettings["videoize"];
        #endregion

        //构造函数
        //创建目录
        public FormatConverter()
        {
        }
        #endregion

        #region 公有成員
        private string destVideo = "";

        /// <summary>
        /// 视频路径
        /// </summary>
        public string DestVideo
        {
            get { return destVideo; }
            set { destVideo = value; }
        }
        private string destImage = "";

        /// <summary>
        /// 图片路径
        /// </summary>
        public string DestImage
        {
            get { return destImage; }
            set { destImage = value; }
        }
        /// <summary>
        /// 视频长度
        /// </summary>
        public string VideoLength { get; set; }
        /// <summary>
        /// 文件類型
        /// </summary>
        public enum VideoType
        {
            [Description(".avi")]
            AVI,
            [Description(".mov")]
            MOV,
            [Description(".mpg")]
            MPG,
            [Description(".mp4")]
            MP4,
            [Description(".flv")]
            FLV
        }
        #endregion

        #region 使用FFmpeg进行格式转换
        /// <summary>
        /// 运行格式转换
        /// </summary>
        /// <param name="sourceFile">要转换文件绝对路径</param>
        /// <param name="destPath">转换结果存储的相对路径</param>
        /// <param name="videotype">要转换成的文件类型</param>
        /// <param name="createImage">是否生成截图</param>
        /// <returns>
        /// 执行成功返回空，否则返回错误信息
        /// </returns>
        public string Convert(string sourceFile, string destPath, string uniquename, VideoType videotype, bool createImage, bool getDuration)
        {
            //取得ffmpeg.exe的物理路径
            string ffmpeg = System.Web.HttpContext.Current.Server.MapPath(ffmpegpath);
            if (!File.Exists(ffmpeg))
            {
                return "找不到格式转换程序！";
            }
            if (!File.Exists(sourceFile))
            {
                return "找不到源文件！";
            }
            //string uniquename = FileHelper.GetUniqueFileName();
            string filename = uniquename + GetDiscription(videotype);
            if (!destPath.EndsWith(@"\"))
                destPath = string.Format("{0}\\", destPath);
            string destFile = HttpContext.Current.Server.MapPath(destPath + filename);
            //if (Path.GetExtension(sourceFile).ToLower() != GetDiscription(videotype).ToLower())
            //{
            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            /*ffmpeg参数说明
             * -i 1.avi   输入文件
             * -ab/-ac <比特率> 设定声音比特率，前面-ac设为立体声时要以一半比特率来设置，比如192kbps的就设成96，转换 
                均默认比特率都较小，要听到较高品质声音的话建议设到160kbps（80）以上
             * -ar <采样率> 设定声音采样率，PSP只认24000
             * -b <比特率> 指定压缩比特率，似乎ffmpeg是自动VBR的，指定了就大概是平均比特率，比如768，1500这样的   --加了以后转换不正常
             * -r 29.97 桢速率（可以改，确认非标准桢率会导致音画不同步，所以只能设定为15或者29.97）
             * s 320x240 指定分辨率
             * 最后的路径为目标文件
             */
            FilestartInfo.Arguments = " -i " + sourceFile + " -ab 80 -ar 22050 -r 29.97 -s " + videosize + " " + destFile;
            //FilestartInfo.Arguments = "-y -i " + sourceFile + " -s 320x240 -vcodec h264 -qscale 4  -ar 24000 -f psp -muxvb 768 " + destFile;
            try
            {
                //转换
                System.Diagnostics.Process.Start(FilestartInfo);
                destVideo = destPath + filename;
            }
            catch
            {
                return "格式转换失败！";
            }
            //}
            //格式不需要转换则直接复制文件到目录
            //else
            //{
            //    File.Copy(sourceFile, destFile,true);
            //    destVideo = destPath + filename;
            //}
            //提取视频长度
            if (getDuration)
            {
                VideoLength = GetVideoDuration(ffmpeg, sourceFile);
            }
            //提取图片
            if (createImage)
            {
                //定义进程
                System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);

                //截大图
                string imgpath = destPath + uniquename + ".jpg";// FileHelper.GetUniqueFileName(".jpg");
                ConvertImage(sourceFile, imgpath, imgsize, ImgstartInfo);

                //截小图
                imgpath = destPath + uniquename + "_thumb.jpg";
                DestImage = ConvertImage(sourceFile, imgpath, "80*80", ImgstartInfo);

            }
            return "";
        }
        /// <summary>
        /// 生成縮略圖
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="imgpath"></param>
        /// <param name="imgsize"></param>
        /// <param name="ImgstartInfo"></param>
        /// <returns></returns>
        private string ConvertImage(string sourceFile, string imgpath, string imgsize, System.Diagnostics.ProcessStartInfo ImgstartInfo)
        {
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            /*参数设置
             * -y（覆盖输出文件，即如果生成的文件（flv_img）已经存在的话，不经提示就覆盖掉了）
             * -i 1.avi 输入文件
             * -f image2 指定输出格式
             * -ss 8 后跟的单位为秒，从指定时间点开始转换任务
             * -vframes
             * -s 指定分辨率
             */
            //duration: 00:00:00.00
            string[] time = VideoLength.Split(':');
            int seconds = int.Parse(time[0]) * 60 * 60 + int.Parse(time[1]) * 60 + int.Parse(time[2]);
            int ss = seconds > 5 ? 5 : seconds - 1;
            ImgstartInfo.Arguments = " -i " + sourceFile + " -y -f image2 -ss " + ss.ToString() + " -vframes 1 -s " + imgsize + " " + HttpContext.Current.Server.MapPath(imgpath);
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo);
                return imgpath;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ffmpegfile"></param>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        private string GetVideoDuration(string ffmpegfile, string sourceFile)
        {
            using (System.Diagnostics.Process ffmpeg = new System.Diagnostics.Process())
            {
                String duration;  // soon will hold our video's duration in the form "HH:MM:SS.UU"
                String result;  // temp variable holding a string representation of our video's duration
                StreamReader errorreader;  // StringWriter to hold output from ffmpeg

                // we want to execute the process without opening a shell
                ffmpeg.StartInfo.UseShellExecute = false;
                //ffmpeg.StartInfo.ErrorDialog = false;
                ffmpeg.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                // redirect StandardError so we can parse it
                // for some reason the output comes through over StandardError
                ffmpeg.StartInfo.RedirectStandardError = true;

                // set the file name of our process, including the full path
                // (as well as quotes, as if you were calling it from the command-line)
                ffmpeg.StartInfo.FileName = ffmpegfile;

                // set the command-line arguments of our process, including full paths of any files
                // (as well as quotes, as if you were passing these arguments on the command-line)
                ffmpeg.StartInfo.Arguments = "-i " + sourceFile;

                // start the process
                ffmpeg.Start();

                // now that the process is started, we can redirect output to the StreamReader we defined
                errorreader = ffmpeg.StandardError;

                // wait until ffmpeg comes back
                ffmpeg.WaitForExit();

                // read the output from ffmpeg, which for some reason is found in Process.StandardError
                result = errorreader.ReadToEnd();

                // a little convoluded, this string manipulation...
                // working from the inside out, it:
                // takes a substring of result, starting from the end of the "Duration: " label contained within,
                // (execute "ffmpeg.exe -i somevideofile" on the command-line to verify for yourself that it is there)
                // and going the full length of the timestamp

                duration = result.Substring(result.IndexOf("Duration: ") + ("Duration: ").Length, ("00:00:00").Length);
                return duration;
            }
        }
        /// <summary>
        /// 返回枚举类型的描述信息
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        private string GetDiscription(System.Enum myEnum)
        {

            System.Reflection.FieldInfo fieldInfo = myEnum.GetType().GetField(myEnum.ToString());
            object[] attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                DescriptionAttribute desc = attrs[0] as DescriptionAttribute;
                if (desc != null)
                {
                    return desc.Description.ToLower();
                }
            }
            return myEnum.ToString();
        }
        //将GetDesCription定义为扩展方法,需.net3.5
        //public static string Description(this Enum myEnum)
        //{
        //    return GetDiscription(myEnum);
        //}
        #endregion

    }
}





