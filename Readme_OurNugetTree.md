This is our tree of nugets, as it will be after the merges in issues 225,226 and 163

```mermaid
  graph TD;
  
      PCAxis.Metadata-->PCAxis.Serializers
      PCAxis.Core-->PCAxis.Serializers
      PCAxis.Core-->PCAxis.Query
      PCAxis.Core-->PCAxis.Sql
      PCAxis.Core --> PCAxis.Search
      PCAxis.Menu --> PCAxis.Search
      PCAxis.Query-->PCAxis.Serializers
      PCAxis.Query-->PXWeb.SavedQuery
      PCAxis.Sql --> PCAxis.Menu
      
      
```
