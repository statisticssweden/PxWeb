using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PCAxis.Query;
using PCAxis.Paxiom;
using PCAxis.Web.Core.Management;

namespace PXWeb.Views
{
    public class FileViewSerializer : IViewSerializer
    {
        protected string _fileFormat;
        
        public FileViewSerializer()
        {

        }

        public FileViewSerializer(string fileFormat)
        {
            _fileFormat = fileFormat;
        }

        public virtual PCAxis.Query.Output Save()
        {
            Output output = new Output();

            output.Type = _fileFormat;

            return output;
        }

        public virtual void Render(string format, PCAxis.Query.SavedQuery query, PCAxis.Paxiom.PXModel model, bool safe)
        {
            //if (!safe)
            //{ 
            //    model.Meta.Notes.Add(new Note(LocalizationManager.GetLocalizedString("PxWebSavedQueryUnsafeMessage"), NoteType.Table, true));
            //}
            var info = PCAxis.Web.Controls.CommandBar.Plugin.CommandBarPluginManager.GetFileType(format);

            //PCAxis.Web.Core.ISerializerCreator creator = Activator.CreateInstance(Type.GetType(info.Creator)) as PCAxis.Web.Core.ISerializerCreator;

            //HttpContext.Current.Response.ContentType = info.MimeType;
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + model.Meta.Matrix + "." + info.FileExtension);

            //var serializer = creator.Create(query.Output.Type);

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    serializer.Serialize(model, ms);
            //    ms.Position = 0;
            //    ms.WriteTo(HttpContext.Current.Response.OutputStream);
            //}
            Render(format, query, model, safe, info.FileExtension, info.MimeType);
        }

        public virtual void Render(string format, PCAxis.Query.SavedQuery query, PCAxis.Paxiom.PXModel model, bool safe, string fileExtension, string mimeType)
        {
            if (!safe)
            {
                model.Meta.Notes.Add(new Note(LocalizationManager.GetLocalizedString("PxWebSavedQueryUnsafeMessage"), NoteType.Table, true));
            }
            var info = PCAxis.Web.Controls.CommandBar.Plugin.CommandBarPluginManager.GetFileType(format);

            PCAxis.Web.Core.ISerializerCreator creator = Activator.CreateInstance(Type.GetType(info.Creator)) as PCAxis.Web.Core.ISerializerCreator;

            if (!mimeType.Contains("json")) // json always use UTF8
            {
                HttpContext.Current.Response.Charset = model.Meta.CodePage;
            }

            HttpContext.Current.Response.ContentType = mimeType;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + model.Meta.Matrix + "." + fileExtension);

            var serializer = creator.Create(format);

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(model, ms);
                ms.Position = 0;
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
            }
        }
    }
}