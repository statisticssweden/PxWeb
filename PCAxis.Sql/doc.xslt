<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>
  
  <xsl:template match="/">
<HTML>
<HEAD>
<TITLE><xsl:value-of select="doc/assembly/name"/></TITLE>
<LINK rel="stylesheet" type="text/css" href="doc.css"/>
Paxiom keywords and CNMM 
</HEAD>
<BODY>
 <table>
  <tbody>
    <tr>
      <th>PXKeyword</th><th>Rule</th><th>Table</th><th>Colomn</th>
    </tr>
     <!-- <xsl:apply-templates select="doc/members/member[PXKeyword]"/> -->
    <xsl:apply-templates select="doc/members/member/PXKeyword"> 
       <xsl:sort select="@name"  />   
    </xsl:apply-templates>
  </tbody>
</table>
    
</BODY>
</HTML>
</xsl:template>


<xsl:template match="PXKeyword">
  <tr>
      <th>
       <xsl:attribute name="rowspan">
        <xsl:value-of select="count(rule/table/column)-count(rule[table]) + count(rule)"/>
        <!--    -count(rule[table]) + count(rule)  =  those who only has description (no tables)     -->
        </xsl:attribute>
        <xsl:value-of select="@name"/>
        </th>
         
     <xsl:apply-templates select="rule"/>
    </tr>
      
  </xsl:template>
  
  
  <xsl:template match="rule">
   <xsl:choose>
       <xsl:when test="position() = 1">  
       
        <td><xsl:attribute name="rowspan">
        <xsl:value-of select="count(table/column)"/>
        </xsl:attribute>
        
        <xsl:value-of select="description"/> 
     </td>
     <xsl:apply-templates select="table"/>
     
     </xsl:when>
     <xsl:otherwise>     
     <tr>
     
     <td><xsl:attribute name="rowspan">
        <xsl:value-of select="count(table/column)"/>
        </xsl:attribute>        
        <xsl:value-of select="description"/> 
     </td>
     <xsl:apply-templates select="table"/>
    
        </tr>    
     </xsl:otherwise>
     </xsl:choose>
  </xsl:template>
  
  
  
   <xsl:template match="table">
       <xsl:choose>
       <xsl:when test="position() = 1">  
       
     <td>
      <xsl:attribute name="rowspan">
        <xsl:value-of select="count(column)"/>
        </xsl:attribute>
     <xsl:value-of select="@modelName"/>
     </td>     
     <xsl:apply-templates select="column">
       <xsl:sort select="@modelName"/>     
     </xsl:apply-templates>
     
  </xsl:when>
     <xsl:otherwise>     
     <tr>
        <td>
      <xsl:attribute name="rowspan">
        <xsl:value-of select="count(column)"/>
        </xsl:attribute>
     <xsl:value-of select="@modelName"/>
     </td>
     <xsl:apply-templates select="column"/>
        </tr>    
     </xsl:otherwise>
     </xsl:choose>
    </xsl:template>
    
    <xsl:template match="column ">
       <xsl:choose>
       <xsl:when test="position() = 1">  
  
            <td>
             <xsl:value-of select="@modelName"/>
             </td>
       </xsl:when>
     <xsl:otherwise>
           <tr>
           <td>
     <xsl:value-of select="@modelName"/>
     </td>
     </tr>    
     </xsl:otherwise>
</xsl:choose>
  
  
    </xsl:template>
    

</xsl:stylesheet>
