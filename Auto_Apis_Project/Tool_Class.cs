using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Auto_Apis_Project
{
    public class Tool_Class
    {
        // 创建文件及内容
        private static void CreateFile(string basePath, string path, string fileName, string requestUrl, string requestType)
        {
            var _createPath = AppDomain.CurrentDomain.BaseDirectory + path + "\\" + fileName + ".js";
            StreamWriter sr;
            if (!File.Exists(_createPath))
            {
                sr = File.CreateText(_createPath);
                sr.WriteLine("import axios from '../config/network';");
                sr.WriteLine("import { vm } from '../main';");
            }
            else
            {
                sr = new StreamWriter(_createPath, true);
            }
            sr.WriteLine("export function " + fileName + "(data) {");
            sr.WriteLine("return new Promise((resolve, reject) => {");
            sr.WriteLine("axios");
            sr.WriteLine("." + requestType + "(`${" + basePath + "}/" + requestUrl + "`, data)");
            sr.WriteLine(".then(res => {");
            sr.WriteLine("resolve(res);");
            sr.WriteLine(" })");
            sr.WriteLine(" .catch (err => {");
            sr.WriteLine(" vm.$error(`请求接口${" + basePath + "}" + requestUrl + "错误`, JSON.stringify(err));");
            sr.WriteLine(" reject(err);");
            sr.WriteLine(" });");
            sr.WriteLine(" });");
            sr.WriteLine("}");
            sr.Close();
        }
        // 请求输入的接口路径
        private static string RequestData(string reqeustUrl)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PostAsync(reqeustUrl, null).Result;
            var staticCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            return null;
        }
        //导出文件
        public static void OutputFile(string letVal, string actionUrl)
        {
            //var outFile = AppDomain.CurrentDomain.BaseDirectory + "OutputFiles";
            //new FileInfo(outFile).Attributes = FileAttributes.Normal;
            //File.Delete(outFile);
            var result = RequestData(actionUrl);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Dictionary<string, Object> dics = js.Deserialize<Dictionary<string, Object>>(result);
            foreach (var item in (Dictionary<string, Object>)dics["paths"])
            {
                foreach (var citem in (Dictionary<string, Object>)item.Value)
                {
                    var _tempReg = new Regex("^/api/\\w*/");
                    var _lastReg = new Regex("/{\\w*}");
                    var filename = _tempReg.Replace(item.Key, "");
                    filename = _lastReg.Replace(filename, "");
                    try
                    {
                        CreateFile(letVal, "OutputFiles", filename, item.Key, citem.Key);
                    }
                    catch { }
                }
                //CreateFile()
            }
        }
    }
}