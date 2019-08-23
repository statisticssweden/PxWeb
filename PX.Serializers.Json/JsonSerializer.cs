using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using PCAxis.Query;


namespace PX.Serializers.Json
{
    public class JsonSerializer : PCAxis.Paxiom.IPXModelStreamSerializer
    {
        public void Serialize(PXModel model, string path)
        {
            Serialize(model, new FileStream(path, FileMode.Create));
        }

        public void Serialize(PXModel model, Stream stream)
        {
            //using (var writer = new StreamWriter(stream, Encoding.UTF8))
            //{
            var writer = new StreamWriter(stream, Encoding.UTF8);
                var tableResponse = new TableResponse();
                PXModel pivotedModel = RearrangeValues(model);
                var formatter = new DataFormatter(pivotedModel);
                formatter.DecimalSeparator = ".";
                formatter.ThousandSeparator = "";

                // Add stub
                tableResponse.Columns.AddRange(pivotedModel.Meta.Stub.Select(s => new TableResponseColumn
                {
                    Code = s.Code,
                    Text = s.Name,
                    Type = s.IsTime ? "t" : "d",
                    Comment = s.HasNotes() ? s.Notes.GetAllNotes() : null
                }));

                if (pivotedModel.Meta.ContentVariable != null)
                {
                    // Add heading
                    tableResponse.Columns.AddRange(pivotedModel.Meta.ContentVariable.Values.Select(val => new TableResponseColumn
                    {
                        Code = val.Code,
                        Text = val.Text,
                        Type = "c",
                        Comment = val.HasNotes() ? val.Notes.GetAllNotes() : null
                    }));
                }
                else
                {
                    tableResponse.Columns.Add(new TableResponseColumn
                    {
                        Code = pivotedModel.Meta.Contents,
                        Text = pivotedModel.Meta.Contents,
                        Type = "c"
                    });
                }

                // Add comments
                foreach (var variable in pivotedModel.Meta.Stub)
                {
                    foreach (var value in variable.Values)
                    {
                        if (value.HasNotes())
                        {
                            tableResponse.Comments.Add(new TableResponseComment
                            {
                                Comment = value.Notes.GetAllNotes(),
                                Value = value.Code,
                                Variable = variable.Code
                            });
                        }
                    }
                }

                int row = 0;
                Build(pivotedModel, formatter, 0, ref row, tableResponse, new List<string>());

           
                // Write to output stream
                writer.Write(tableResponse.ToJSON(false));
                writer.Flush();
           // } End using
        }

        /// <summary>
        /// Builds the table response data object recursively.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="formatter"></param>
        /// <param name="varIdx"></param>
        /// <param name="row"></param>
        /// <param name="response"></param>
        /// <param name="key"></param>
        private void Build(PXModel model, DataFormatter formatter, int varIdx, ref int row, TableResponse response, List<string> key)
        {
            foreach (var value in model.Meta.Stub[varIdx].Values)
            {
                if (varIdx + 1 < model.Meta.Stub.Count)
                {
                    // Continue building
                    Build(model, formatter, varIdx + 1, ref row, response, new List<string>(key) { value.Code });
                }
                else
                {
                    // No more variables. Output key and data
                    var data = new TableResponseData
                    {
                        Key = new List<string>(key) { value.Code }
                    };

                    for (int col = 0; col < model.Data.MatrixColumnCount; col++)
                    {
                        data.Values.Add(formatter.ReadElement(row, col));
                    }
                    response.Data.Add(data);
                    row++;
                }
            }
        }

        /// <summary>
        /// Pivots the data to fit the output format
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private PXModel RearrangeValues(PXModel model)
        {
            var nonContentVariables = model.Meta.Variables.Where(v => v.IsContentVariable == false);
            var pivotDescriptions = nonContentVariables.Select(cv => new PivotDescription(cv.Name, PlacementType.Stub)).ToList();
            if (model.Meta.ContentVariable != null)
            {
                pivotDescriptions.Add(new PivotDescription(model.Meta.ContentVariable.Name, PlacementType.Heading));
            }
            return new Pivot().Execute(model, pivotDescriptions.ToArray());
        }
        #region IWebSerializer Members


        //public void Serialize(PCAxis.Paxiom.PXModel model, ResponseBucket cacheResponse)
        //{
        //    cacheResponse.ContentType = "application/json; charset=" + System.Text.Encoding.UTF8.WebName;

        //    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        Serialize(model, stream);
        //        stream.Flush();
        //        cacheResponse.ResponseData = stream.ToArray();
        //    }
        //}

        #endregion

    }
}
