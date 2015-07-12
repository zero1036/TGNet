using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Yahoo.Yui.Compressor;

namespace TG.Example
{
    public class AutoCompress
    {
        string basePath = @"C:\Users\Administrator\Desktop\WebApplication1\WebApplication1\";

        public bool Compress()
        {
            bool retVal = true;

            //string filePath = basePath + "myjs";
            string filePath = @"D:\";
            string jsContent = ""; ;

            CompressJsAndCss(filePath, ref jsContent);

            File.WriteAllText(basePath + "my_mini.js", jsContent.ToString(), Encoding.UTF8);

            return retVal;
        }

        /// <summary>
        /// 功能：压缩特定路径下的js、css文件
        /// 执行过程：
        /// (1)、获取文件夹底下的所有js以及css文件
        /// (2)、将文件压缩后保存成.au.min.js或auto.min.css格式
        /// 
        /// 注意：若是js文件中的js有顺序，需要整理一下顺序再压缩，
        /// 因为全部压缩成功后，全部js会放到一个js文件中
        /// 
        /// </summary>
        /// <param name="filePath">文件夹路径</param>
        /// <param name="jsContent">将文件夹下的所有js压缩后的内容</param>
        /// <returns></returns>

        private bool CompressJsAndCss(string filePath, ref string jsContent)
        {
            bool ret = true;

            //目录不存在

            if (!Directory.Exists(filePath))
            {
                return false;
            }

            //获取该文件夹下所有js或者css文件
            var fileInfos = new List<FileInfo>();

            GetJsAndCssFileList(new DirectoryInfo(filePath), ref fileInfos);
            foreach (FileInfo info in fileInfos)
            {
                try
                {
                    //压缩文件保存路径

                    var newFile = new FileInfo(info.FullName.Replace(info.Name, info.Name.Replace(info.Extension, ".auto.min" + info.Extension)));

                    //文件内容
                    string strContent = File.ReadAllText(info.FullName, Encoding.UTF8);

                    //若该文件为js文件
                    if (info.Extension.ToLower() == ".js")
                    {
                        if (!info.FullName.ToLower().EndsWith(".no.js"))
                        {
                            //若该文件已经压缩则不进行压缩
                            if (!info.FullName.ToLower().EndsWith(".min.js"))
                            {
                                //初始化
                                var js = new JavaScriptCompressor(strContent, false, Encoding.UTF8,

                                System.Globalization.CultureInfo.CurrentCulture);

                                //压缩该js
                                strContent = js.Compress();

                                //File.WriteAllText(newFile.FullName, strContent, Encoding.UTF8);
                            }

                            //文件之间进行换行
                            //jsContent.Append(strContent + " \n\n");

                            jsContent += strContent + " \n\n";
                        }
                    }
                    else if (info.Extension.ToLower() == ".css") //该文件为css文件
                    {
                        //若为不需要的css则不进行压缩
                        if (!info.FullName.ToLower().EndsWith(".no.css"))
                        {
                            //若已经进行过压缩，则不再进行压缩
                            if (!info.FullName.ToLower().EndsWith(".min.css"))
                            {
                                //进行压缩
                                strContent = CssCompressor.Compress(strContent);
                                //File.WriteAllText(newFile.FullName, strContent, Encoding.UTF8);
                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    ret = false;
                    continue;
                }
            }

            //File.WriteAllText(basePath + "DragHidden_mini.js", jsContent, Encoding.UTF8);


            return ret;
        }

        /// <summary>
        /// 功能：获取CSS以及JS文件列表    /// 
        /// </summary>
        /// <param name="dir">目标目录</param>
        /// <param name="fileList">文件列表</param>
        private void GetJsAndCssFileList(DirectoryInfo dir, ref List<FileInfo> fileList)
        {
            FileInfo[] files = dir.GetFiles("my.js");

            //FileInfo p = new FileInfo();

            fileList.AddRange(files);

            //files = dir.GetFiles("*.css");

            //fileList.AddRange(files);

            //DirectoryInfo[] dirs = dir.GetDirectories();

            //if (dirs.Length > 0)
            //{
            //    foreach (DirectoryInfo r in dirs)
            //    {
            //        GetJsAndCssFileList(r, ref fileList);
            //    }
            //}
        }
    }
}
