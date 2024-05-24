namespace PCAxis.Html5Table.Web.Controls
{

    public class Html5TableSerializerCreator : PCAxis.Web.Core.ISerializerCreator
    {

        public Paxiom.IPXModelStreamSerializer Create(string fileInfo)
        {
            PCAxis.Serializers.Html5TableSerializer ser;
            ser = new PCAxis.Serializers.Html5TableSerializer();

            return ser;
        }
    }
}
