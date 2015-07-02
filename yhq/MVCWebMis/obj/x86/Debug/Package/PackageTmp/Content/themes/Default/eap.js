
//------------------ϵͳ���õ��ķ���---------------
//�����ĳ�������ϵ����ѡ�и��У�
function dataline_click(obj)
{
   try
   {
      var datagrid = document.getElementById('datagrid');
      for(var i=0;i<datagrid.rows.length;i++)
      {
         if(datagrid.rows[i].className=='dataline-click')
         {
            if(i%2==0) datagrid.rows[i].className='datalineone';
            else datagrid.rows[i].className='datalinetwo';
         }
      }
      obj.className = "dataline-click";	
   }
   catch(ex)
   {}
}
//����ƶ�����������
function dataline_over(obj)
{
   try
   {
      if(obj.className=="datalineone") obj.className="dataline-over1";
      if(obj.className=="datalinetwo") obj.className="dataline-over2";
   }
   catch(ex)
   {}
}
//���Ӹ����������ƿ�
function dataline_out(obj) 
{
   try
   {
	   if(obj.className=="dataline-over1") obj.className="datalineone";
	   if(obj.className=="dataline-over2") obj.className="datalinetwo";
   }
   catch(ex)
   {}
}

//��UTF16תUTF8(JavaScript�л�õ������ַ�����UTF16���б���ģ���ͳһ��ҳ���׼��ʽ��UTF-8�ɲ�һ��Ŷ��������Ҫ�Ƚ���ת��UTF-16��UTF8)
function utf16to8(str)
{
   var out, i, len, c;
   out = "";
   len = str.length;
   for(i = 0; i < len; i++) 
   {
      c = str.charCodeAt(i);
      if((c >= 0x0001) && (c <= 0x007F)) 
      {
         out += str.charAt(i);
      }
      else if (c > 0x07FF) 
      {
         out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
         out += String.fromCharCode(0x80 | ((c >>  6) & 0x3F));
         out += String.fromCharCode(0x80 | ((c >>  0) & 0x3F));
       }
      else 
      {
         out += String.fromCharCode(0xC0 | ((c >>  6) & 0x1F));
         out += String.fromCharCode(0x80 | ((c >>  0) & 0x3F));
      }
   }
   return out;
}
//��UTF8תUtf16��
function utf8to16(str)
{
   var out, i, len, c;
   var char2, char3;
   out = "";
   len = str.length;
   i = 0;
   while(i < len) 
   {
      c = str.charCodeAt(i++);
      switch(c >> 4)
      {
         case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
           // 0xxxxxxx
           out += str.charAt(i-1);
           break;
         case 12: case 13:
           // 110x xxxx   10xx xxxx
           char2 = str.charCodeAt(i++);
           out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
           break;
         case 14:
           // 1110 xxxx  10xx xxxx  10xx xxxx
           char2 = str.charCodeAt(i++);
           char3 = str.charCodeAt(i++);
           out += String.fromCharCode(((c & 0x0F) << 12) |
              ((char2 & 0x3F) << 6) |
              ((char3 & 0x3F) << 0));
           break;
      }
   }
   return out;
}

//���ַ���inputstr���м��ܣ������ؼ��ܺ��ַ���
function encode(inputstr)
{
   var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
   if(inputstr=="")return "AA==";
   inputstr = escape(inputstr);
   var output = "";
   var chr1, chr2, chr3 = "";
   var enc1, enc2, enc3, enc4 = "";
   var i = 0;
   do
   {
      chr1 = inputstr.charCodeAt(i++);
      chr2 = inputstr.charCodeAt(i++);
      chr3 = inputstr.charCodeAt(i++);
      enc1 = chr1 >> 2;
      enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
      enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
      enc4 = chr3 & 63;
      if(isNaN(chr2)){ enc3 = enc4 = 64; }
      else if(isNaN(chr3)){ enc4 = 64; }
      output = output + keyStr.charAt(enc1) + keyStr.charAt(enc2) + keyStr.charAt(enc3) + keyStr.charAt(enc4);
      chr1 = chr2 = chr3 = "";
      enc1 = enc2 = enc3 = enc4 = "";
   } 
   while (i < inputstr.length);
   return output;
}

//���ַ�������inputstr���ܣ������ؽ��ܺ���ַ���
function decode(inputstr)
{
   var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
   if(inputstr=="AA==")return "";
   var output = "";
   var chr1, chr2, chr3 = "";
   var enc1, enc2, enc3, enc4 = "";
   var i = 0;
   // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
   var base64test = /[^A-Za-z0-9\+\/\=]/g;
   if(base64test.exec(inputstr))
   {
      /* alert("There were invalid base64 characters in the inputstr text.\n" +
         "Valid base64 characters are A-Z, a-z, 0-9, '+', '/', and '='\n" +
         "Expect errors in decoding.");*/
   }
   inputstr = inputstr.replace(/[^A-Za-z0-9\+\/\=]/g, "");
   do
   {
      enc1 = keyStr.indexOf(inputstr.charAt(i++));
      enc2 = keyStr.indexOf(inputstr.charAt(i++));
      enc3 = keyStr.indexOf(inputstr.charAt(i++));
      enc4 = keyStr.indexOf(inputstr.charAt(i++));
      chr1 = (enc1 << 2) | (enc2 >> 4);
      chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
      chr3 = ((enc3 & 3) << 6) | enc4;
      output = output + String.fromCharCode(chr1);
      if(enc3 != 64){ output = output + String.fromCharCode(chr2); }
      if(enc4 != 64){ output = output + String.fromCharCode(chr3); }
      chr1 = chr2 = chr3 = "";
      enc1 = enc2 = enc3 = enc4 = "";
   } 
   while (i < inputstr.length);
   return unescape(output);
}

//����ajax��Request����
function initAjax()
{
   var ajax_request=false; //��false�Ż�Ϊnull?
   try
   {
      ajax_request = new ActiveXObject("Msxml2.XMLHTTP");
   }
   catch(ex)
   {
      try
      { 
         ajax_request = new ActiveXObject("Microsoft.XMLHTTP");  
      }
      catch(ex)
      { 
         ajax_request = false; 
      }
   }
   if(!ajax_request && typeof XMLHttpRequest!='undefined') 
   {
      ajax_request = new XMLHttpRequest();
   }
   return ajax_request;
}
//��ȡ����xml
function getresponsexml(urlstr)
{
   var xmlDoc=null;
   try
   {

      var http_request=false;
      http_request=initAjax();
      http_request.onreadystatechange = function()
      {
         if (http_request.readyState == 4) 
         {
            xmlDoc = http_request.responseXML;
         }
      }
      http_request.open("GET", urlstr, false);
      http_request.send(null);
   }
   catch(ex1)
   {
      xmlDoc=null;
   }
   return xmlDoc;
}
//��ȡ���ص�Ajax�ַ���
//url:����urlstr��÷����ַ���
//̷���޸�
function getresponseText(urlstr){
      var xmlDoc=null;
   try{

      var http_request=false;
      http_request=initAjax();
    http_request.onreadystatechange = function(){
          if (http_request.readyState == 4) {
            xmlDoc = http_request.responseText;
       }
      }
      http_request.open("GET", urlstr, false);
      http_request.send(null);
   }catch(ex1){
         xmlDoc=null;
      }
      return xmlDoc;
}
//��ȡ���ص�Ajax�ַ���
//url:����urlstr��÷����ַ���
function getAjaxText(url)
{
   var ajaxText="";
   try
   {
      var http_request=initAjax();
      http_request.onreadystatechange=function()
      {
         if(http_request.readyState==4){ ajaxText = Trim(http_request.responseText); return ajaxText; }
      }
      http_request.open("GET", url, false);
      http_request.send(null);
      ajaxText = Trim(http_request.responseText);
   }
   catch(ex)
   {
      ajaxText="����ajax����";
   }
   return ajaxText;
}
//��ʾĳ����(action)�Ľ�������������ʾ��ϢΪmessage��message������ʡ�ԣ�action�������鲻Ҫʡ�ԡ�
var progress_stop=1,progress_element=null;
function showProgress(message)
{
   try
   {
      var element = event.srcElement;
      if(progress_stop==0) return true;
      progress_stop=0;
      progress_element=element;
      if(element!=null)
      { 
         element.disabled=true; 
         if(element.type=="submit")element.form.submit(); 
      }
      if(message!=null) 
      {
         progress_text.innerHTML=message;
      }
      setTimeout("showProgressWidth(10)",500);
   }
   catch(ex)
   { 
      alert("ִ��Javascript�ű�����"); 
   }
   return true;
}
function showProgressWidth(wid)
{
   try
   {
      progress_width.style.width=wid ;
      newWid=wid+10;
      if(newWid>=800) newWid=0;
      progress_line.style.display='';
      if(progress_stop==0) setTimeout("showProgressWidth(newWid)", 100);
      else
      {
         if(progress_element!=null) progress_element.disabled=false;
         progress_line.style.display='none';
      }
   }
   catch(ex)
   { 
      alert("ִ��Javascript�ű�����"); 
   }
}

//����PrintControl.ExecWB(?,?)ʵ��ֱ�Ӵ�ӡ�ʹ�ӡԤ�����ܡ�(ֱ����ϵͳ�ṩ��print()������ӡ�޷�����ĳЩ����)
//preview���Ƿ���ʾԤ����null/false:����ʾ��true:��ʾ
function printPage(preview)
{
   try
   {
      var content=window.document.body.innerHTML;
      var oricontent=content;
      while(content.indexOf("{$printhide}")>=0) content=content.replace("{$printhide}","style='display:none'");
      if(content.indexOf("ID=\"PrintControl\"")<0) content=content+"<OBJECT ID=\"PrintControl\" WIDTH=0 HEIGHT=0 CLASSID=\"CLSID:8856F961-340A-11D0-A96B-00C04FD705A2\"></OBJECT>";
      window.document.body.innerHTML=content;
      //PrintControl.ExecWB(7,1)��ӡԤ����(1,1)�򿪣�(4,1)���Ϊ��(17,1)ȫѡ��(10,1)���ԣ�(6,1)��ӡ��(6,6)ֱ�Ӵ�ӡ��(8,1)ҳ������
      if(preview==null||preview==false) PrintControl.ExecWB(6,1);
      else PrintControl.ExecWB(7,1); //OLECMDID_PRINT=7; OLECMDEXECOPT_DONTPROMPTUSER=6/OLECMDEXECOPT_PROMPTUSER=1
      window.document.body.innerHTML=oricontent;
   }
   catch(ex)
   { 
      alert("ִ��Javascript�ű�����"); 
   }
}

//�Ѹý��(numeric)ת��Ϊ��д�����Ľ��
function capitalNum(numeric)
{
   try
   {
      var n = numeric;
      var strOutput = "";
      var strUnit = 'Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�';
      n += "00";
      var intPos = n.indexOf('.');
      if (intPos >= 0) n = n.substring(0, intPos) + n.substr(intPos + 1, 2);
      strUnit = strUnit.substr(strUnit.length - n.length);
      for (var i=0; i < n.length; i++) strOutput += '��Ҽ��������½��ƾ�'.substr(n.substr(i,1),1) + strUnit.substr(i,1);
      return strOutput;
   }
   catch(ex)
   { 
      return ""; 
      alert("ִ��Javascript�ű�����"); 
   }
}

// �ѵ��еĲ˵���������Ϊmenuclick����Ϊѡ��״̬�������ϴε��еĲ˵���������Ϊmenu���ָ�Ϊ��ʼ״̬��
var lastMenu;
var lastModule;
document.onclick = menu_click;
function menu_click()
{

  var srcElement=window.event.srcElement;
  if(srcElement.className=="menu")
  {
     if(lastMenu!=null) lastMenu.className="menu";
     srcElement.className="menuclick";
     lastMenu=srcElement;
   }
  if(srcElement.className=="modulemove")
  {
     if(lastModule!=null) lastModule.className="module";
     srcElement.className="moduleclick";
     lastModule=srcElement;
   }
}

//����״�ṹ�йصĵ������(��һ�ξ�չ�����ٵ�һ�ξ��۵�)
//obj:<tr>����
//img:�ļ���ͼƬǰ��[�Ӻ�]/[����]��־��
//opt:�򿪵��ļ��б�־��رյ��ļ��б�־��
//flag:�ļ��д�/�رձ�־��(-1:ȫ���򿪣�0:�л�״̬��1:ȫ���ر�)
//path:�õ���ͼƬ��·��
function folder_turnit(obj,img,opt,flag,path)
{
   display=obj.style.display;
   if((flag==-1)||(flag==0 && display=="none"))
   { 
      obj.style.display=""; img.src=path+"images/-.gif"; opt.src=path+"images/dir_f2.gif"; 
   }
   if((flag==+1)||(flag==0 && display!="none"))
   { 
      obj.style.display="none";  img.src=path+"images/+.gif"; opt.src=path+"images/dir_f1.gif"; 
   }
}

//��������������"chk???"��checkbox��ֵ����Ϊ��[chkall]��ֵ��ͬ
//form:Ҫ��������check�ؼ���form����
function checkall()
{
   var element = event.srcElement;
   for(var ii=0;ii<element.form.elements.length;ii++)
   {
      var e = element.form.elements[ii];
      if(e!=element && e.type=="checkbox" && e.id.indexOf('chk')==0) e.checked=element.checked;
   }
}

//�����û�/Ⱥ��Ȩ��ʱ����ѡ��ĳȨ�޺��Զ����������ϼ����¼�Ȩ��
function selectright()
{
   var element = event.srcElement;
   for(var ii=0;ii<element.form.elements.length;ii++)
   {
      var e = element.form.elements[ii];
      //��Ȩ�޵���һ��Ȩ������Ϊ��ͬ
      if(e.id.indexOf(element.id)==0) e.checked = element.checked;
      //����Ȩ��Ϊtrueʱ����Ȩ�޵��ϼ�Ȩ������Ϊtrue
      if(e.type=="checkbox" && element.checked && e.id!='chkall' && element.id.indexOf(e.id)==0) e.checked = element.checked;
   }
}

//ȷ���Ƿ�ȷʵҪ�ύ
function confirmsubmit()
{
   if(confirm('ȷʵҪ�ύ��')) 
   {
      return true;
   }
   else
   {
      return false;
   }
}

//ȷ���Ƿ�ȷʵҪɾ��(ɾ��������¼��ɾ�����м�¼��ɾ��ָ��������¼)
function confirmdelete()
{
   if(confirm('ȷʵҪɾ����¼��')) 
   {
      return true;
   }
   else
   {
      return false;
   }
}

//ȷ���Ƿ���ѡ�еļ�¼����ȷ���Ƿ�Ҫִ�иò�����
function confirm_selected()
{
   var element = event.srcElement;
   selected=false;
   for(var ii=0;ii<element.form.elements.length;ii++)
   {
      var e = element.form.elements[ii];
      if(e.type=="checkbox" && e.id!='chkall' && e.id.indexOf('chk')==0 && e.checked)  selected=true;
   }
   if(!selected)
   { 
      alert("����δѡ���¼����ѡ��"); return false; 
   }
   if(confirm('�Ƿ�ȷʵҪ��ѡ�е����м�¼ִ�иò�����')) 
   {  
      return true;
   }
   return false;
}
//��鱸ע�����볤��
function textCount(obj,a_limit)
{
   if (obj.value.length > a_limit) 
   {   
      alert("���ܳ���" + a_limit + "�ַ���")
      obj.value = obj.value.substring(0,a_limit);
   }
}
//��mystr�ĵ�pos��λ�����ַ������ָ���Ϊdelimiter�� ���mystr,delimiter����null����ַ�������pos<1,�򷵻ؿ��ַ�����
function fieldGet(mystr,pos,delimiter)
{
   // �������ַ���Ϊ��,��������С��1,�򷵻ؿ��ַ���
   if(mystr==null||delimiter==null||mystr==""||delimiter=="" || pos < 1) return "";
   // ѭ������,��ָ��ָ��Ҫ��ȡ�����ʼλ��
   index=mystr.indexOf(delimiter);
   if (index < 0 && pos==1) return mystr;
   count = 1;
   pindex=0;
   leng=delimiter.length;
   while(index>=0 && count<pos){
      count = count + 1;
      pindex = index + leng;
      index = mystr.indexOf(delimiter,index + leng);
      }
   //
   if(count < pos) return "";
   if (index >= 0) return mystr.substring( pindex, index);  //ȥ����index-pindex
   return mystr.substring( pindex);
}//end method

// ����mystr�����������ָ���Ϊdelimiter�����mystr,delimiter����null����ַ���,�򷵻�1����С�ķ���ֵΪ1��
function fieldCount(mystr,delimiter)
{
   // �������ַ���Ϊ��,�򷵻�����ĿΪ1
   if(mystr==null||delimiter==null||mystr=="" || delimiter=="") return 1;
   // ѭ������,��ָ��ָ��Ҫ��ȡ�����ʼλ��
   count = 1;
   leng=delimiter.length;
   index=mystr.indexOf(delimiter);
   while(index >= 0)
   {
      count = count + 1;
      index=mystr.indexOf(delimiter,index+leng);
   }
   return count;
}//end method

//���ݱ������಻ͬ��ʾ��ͬ�����ò���
function showReport(rptTitle,myElement)
{
   try{
   var element = myElement==null?event.srcElement:myElement;
   var frmMain = myElement==null?event.srcElement.form:myElement.form;
   //�������ѡ�������࣬����ʾ��Ӧ�Ľ���
   if(element.name=="report_sort"){
      //����Ҫִ�е��ǲ�ѯ/ͳ��/ͼ��������Ҫ��ʾ������
      if(frmMain.report_sort.value.indexOf("q")==0){
         set_order_cond.style.display='';
         set_group_cond.style.display='none';
         set_total_cond.style.display='none';
         set_chart_cond.style.display='none';
         }
      if(frmMain.report_sort.value.indexOf("g")==0){
         set_order_cond.style.display='none';
         set_group_cond.style.display='';
         set_total_cond.style.display='';
         set_chart_cond.style.display='none';
         }
      if(frmMain.report_sort.value.indexOf("c")==0){
         set_order_cond.style.display='none';
         set_group_cond.style.display='none';
         set_total_cond.style.display='none';
         set_chart_cond.style.display='';
         //����ͼ����z�����ʾ��Ϣ
         var group_field=getGroupField();
         var chart_field=getChartField();
         for(var ii=0;ii<3;ii++){
           chartz=document.getElementsByName("chartz")[ii];
           //chartz=document.getElementById("chartz"+ii);
           if(ii==0) chartz.options.length = 0;
           else  chartz.options.length = 1;
           count=fieldCount(group_field,",");
           for(var jj=1;jj<=count;jj++){
              var text1=fieldGet(group_field,jj,",");
              count2=fieldCount(chart_field,",");
              for(var kk=1;kk<=count2;kk++){
                 var text2=fieldGet(chart_field,kk,",");
                 if(text1.indexOf(text2)>=0){
                    var no = new Option();
                    no.value=fieldGet(text1,1," as ");
                    no.text =text2;
                    chartz.options[chartz.options.length] = no;
                    }
                 }
              }
           }
         }
      //ִ�н������ʾ��ʽ
      if(frmMain.report_sort.value.indexOf("c")==0){ fileext_obj.innerHTML="<select name=fileext>"+fileext_obj_chart.innerHTML; }
      else{  fileext_obj.innerHTML="<select name=fileext>"+fileext_obj_data.innerHTML; }
      //��������ı����Ԥ���屨��ı���
      if(rptTitle==null) rptTitle="";
      frmMain.report_title.value=rptTitle+frmMain.report_sort.options[frmMain.report_sort.selectedIndex].text;
      frmMain.report_name.value=frmMain.report_title.value;
      }
   //��������˱���ͳ�Ʋ�ѯ������ʾ�������
   if(frmMain.save_report.checked) set_save_cond.style.display='';
   else set_save_cond.style.display='none';
   //
   }catch(ex){ alert("ִ��Javascript�ű�����"); }
}

//����ͳ�Ʋ�ѯ�������õ����ֶ�����
function procReport()
{
   try{
   var field_cond="",group_cond="",order_cond="",record_cond="",group_field="",total_field="";
   var field_cond_info="",group_cond_info="",order_cond_info="",record_cond_info="",group_field_info="",total_field_info="";
   var frmMain = event.srcElement.form;
   //�������ʾ��ҳ���ϣ�����´��ڡ�����Ǳ��浽�ļ��У����ڵ�ǰ������ʾ
   if(frmMain.report_sort.value.indexOf("c")==0||frmMain.fileext.value=="") frmMain.target="_blank";
   else frmMain.target="_action";
   //�����ѯ�������Ϣ
   if(frmMain.report_sort.value.indexOf("q")==0){
      //��ʾ���ֶ���Ϣ
      var fieldtemp=frmMain.field_query;
      if(fieldtemp.length<=0) fieldtemp=frmMain.field_all;
      for(var ii=0; ii<fieldtemp.length; ii++){
         if(fieldtemp.options[ii].value=="") continue;
         if(field_cond!=""){ field_cond=field_cond+","; field_cond_info=field_cond_info+","; }
         field_cond=field_cond+fieldtemp.options[ii].value+" as '"+fieldtemp.options[ii].text+"'";
         field_cond_info=field_cond_info+fieldtemp.options[ii].text;
         }
      //������Ϣ
      for(var ii=0;ii<frmMain.elements.length-2;ii++){
         var e = frmMain.elements[ii];
         var e1 = frmMain.elements[ii+1];
         var e2 = frmMain.elements[ii+2];
         if(e.name!="queryorder"||e1.name!="queryorder2"||e.value==null||e1.value==null||e.value==""||e1.value=="") continue;
         if((","+order_cond+",").indexOf(","+e.value+" ")>=0) continue;
         if(order_cond!=""){ order_cond=order_cond+","; order_cond_info=order_cond_info+","; }
         order_cond=order_cond+e.value+" "+e1.value;
         order_cond_info=order_cond_info+e.options[e.selectedIndex].text+" "+e1.options[e1.selectedIndex].text;
         //�����ֶ�
         if(e2.name!="querytotal"||e2.value==null||e2.value=="") continue;
         if(group_field!="") group_field=group_field+",";
         group_field=group_field+e.options[e.selectedIndex].text;
         }
      }
   //����ͳ�Ƶ������Ϣ
   if(frmMain.report_sort.value.indexOf("g")==0){
      for(var ii=0;ii<frmMain.elements.length;ii++){
         var e = frmMain.elements[ii];
         if(e.name!="groupby"||e.value==null||e.value=="") continue;
         //��ʾ���ֶ���Ϣ
         if((","+field_cond+",").indexOf(","+e.value+" ")<0){
            if(field_cond!=""){ field_cond=field_cond+","; field_cond_info=field_cond_info+","; }
            field_cond=field_cond+e.value+" as '"+e.options[e.selectedIndex].text+"'";
            field_cond_info=field_cond_info+e.options[e.selectedIndex].text;
            }
         //group by�ֶ���Ϣ
         if((","+group_cond+",").indexOf(","+e.value+",")<0){
            if(group_cond!=""){ group_cond=group_cond+","; group_cond_info=group_cond_info+","; }
            group_cond=group_cond+e.value;
            group_cond_info=group_cond_info+e.options[e.selectedIndex].text;
            }
         //����/������Ϣ
         if(ii+1>=frmMain.elements.length) continue;
         if(ii+2>=frmMain.elements.length) continue;
         var e1 = frmMain.elements[ii+1]; //�����ֶ�
         var e2 = frmMain.elements[ii+2]; //�����ֶ�(�����˷�����ֶΣ����Զ�������)
         //������Ϣ
         if(e1.name=="grouporder"&&e1.value!=null&&(e1.value!=""||e2.value!="")&&(","+order_cond+",").indexOf(","+e.value+" ")<0){
            if(order_cond!=""){ order_cond=order_cond+","; order_cond_info=order_cond_info+","; }
            order_cond=order_cond+e.value+" "+e1.value;
            order_cond_info=order_cond_info+e.options[e.selectedIndex].text+" "+e1.options[e1.selectedIndex].text;
            }
         //������Ϣ
         if(e2.name=="grouptotal"&&e2.value!=null&&e2.value!=""&&(","+group_field+",").indexOf(","+e.options[e.selectedIndex].text+",")<0){
            if(group_field!="") group_field=group_field+",";
            group_field=group_field+e.options[e.selectedIndex].text;
            }
         }
      //����������Ϣ
      for(var ii=0;ii<frmMain.elements.length;ii++){
         var e = frmMain.elements[ii];
         if(e.name!="grouporder2"||e.value==null||e.value=="") continue;
         if(order_cond!=""){ order_cond=order_cond+","; order_cond_info=order_cond_info+","; }
         order_cond=order_cond+e.value;
         order_cond_info=order_cond_info+e.options[e.selectedIndex].text;
         }
      //ͳ��ʱ��ʾ���ֶ�
      var newfield=getGroupField();
      if(newfield==null) newfield="";
      if(field_cond!="" && newfield!="") field_cond=field_cond+",";
      field_cond=field_cond+newfield;
      }
   //����ͼ��������Ϣ
   if(frmMain.report_sort.value.indexOf("c")==0){
      for(var ii=0;ii<frmMain.elements.length;ii++){
         var e = frmMain.elements[ii];
         if(e.name!="chartx"&&e.name!="charty"&&e.name!="chartz"||e.value==null||e.value=="") continue;
         //��ʾ���ֶ���Ϣ
         if((","+field_cond+",").indexOf(","+e.value+" ")<0){
            if(field_cond!=""){ field_cond=field_cond+","; field_cond_info=field_cond_info+","; }
            field_cond=field_cond+e.value+" as '"+e.options[e.selectedIndex].text+"'";
            field_cond_info=field_cond_info+e.options[e.selectedIndex].text;
            }
         //group by�ֶ���Ϣ
         if((e.name=="chartx"||e.name=="charty")&&(","+group_cond+",").indexOf(","+e.value+",")<0){
            if(group_cond!=""){ group_cond=group_cond+","; group_cond_info=group_cond_info+","; }
            group_cond=group_cond+e.value;
            group_cond_info=group_cond_info+e.options[e.selectedIndex].text;
            }
         //������Ϣ
         if(ii+1>=frmMain.elements.length) continue;
         var e1 = frmMain.elements[ii+1];
         if(e1.name.indexOf("chartorder")==0&&e1.value!=null&&e1.value!=""&&(","+order_cond+",").indexOf(","+e.value+" ")<0){
            if(order_cond!=""){ order_cond=order_cond+","; order_cond_info=order_cond_info+","; }
            order_cond=order_cond+e.value+" "+e1.value;
            order_cond_info=order_cond_info+e.options[e.selectedIndex].text+" "+e1.options[e1.selectedIndex].text;
            }
         }
      }
   //��������
   if(field_cond=="") field_cond="*";
   if(group_cond!="") group_cond="group by "+group_cond;
   if(order_cond!="") order_cond="order by "+order_cond;
   record_cond=frmMain.max_record.value;
   total_field=getTotalField();
   if(record_cond!="") record_cond_info="��ʾǰ "+record_cond+" ����¼";
   if(group_field!="") group_field_info="�� "+group_field+" ����ͳ��";
   if(total_field!="") total_field_info="ͳ�� "+total_field+" ��ֵ";
   if(field_cond_info!="") field_cond_info="��ʾ "+field_cond_info+" �ֶ�";
   if(group_cond_info!="") group_cond_info="ͳ�� "+group_cond_info+" �ֶ�";
   if(order_cond_info!="") order_cond_info="�� "+order_cond_info+" ��ʽ����";
   //��������
   frmMain.fieldCond.value=field_cond; frmMain.fieldCondInfo.value=field_cond_info;
   frmMain.groupCond.value=group_cond; frmMain.groupCondInfo.value=group_cond_info;
   frmMain.orderCond.value=order_cond; frmMain.orderCondInfo.value=order_cond_info;
   frmMain.recordCond.value=record_cond; frmMain.recordCondInfo.value=record_cond_info;
   frmMain.groupField.value=group_field; frmMain.groupFieldInfo.value=group_field_info;
   frmMain.totalField.value=total_field; frmMain.totalFieldInfo.value=total_field_info;
   }catch(ex){ alert("ִ��Javascript�ű�����"); }
}

//��list��ѡ�еļ�¼��������
//list��Ҫ�����ƶ��ļ�¼���ڵ��б����
function move_list_up(list)
{
   var selected=false;
   var tlen=list.options.length;
   for(var ii=0; ii<tlen; ii++){
      if(list.options[ii].selected) selected=true;
      if(ii<=0) continue;
      if(list.options[ii-1].selected||!list.options[ii].selected) continue;
      var newOption = new Option();
      newOption.value = list.options[ii-1].value;
      newOption.text  = list.options[ii-1].text;
      newOption.selected  = list.options[ii-1].selected;
      list.options[ii-1].value =list.options[ii].value;
      list.options[ii-1].text  =list.options[ii].text;
      list.options[ii-1].selected =list.options[ii].selected;
      list.options[ii].value =newOption.value;
      list.options[ii].text  =newOption.text;
      list.options[ii].selected  =newOption.selected;
      }
   if(!selected){ alert("����δѡ��Ҫ�ƶ��ļ�¼����ѡ��"); return false; }
}

//��list��ѡ�еļ�¼��������
//list��Ҫ�����ƶ��ļ�¼���ڵ��б����
function move_list_down(list)
{
   var selected=false;
   var tlen=list.options.length;
   for(var ii=tlen-1; ii>=0; ii--){
      if(list.options[ii].selected) selected=true;
      if(ii>=tlen-1) continue;
      if(list.options[ii+1].selected||!list.options[ii].selected) continue;
      var newOption = new Option();
      newOption.value = list.options[ii+1].value;
      newOption.text  = list.options[ii+1].text;
      newOption.selected  = list.options[ii+1].selected;
      list.options[ii+1].value =list.options[ii].value;
      list.options[ii+1].text  =list.options[ii].text;
      list.options[ii+1].selected =list.options[ii].selected;
      list.options[ii].value =newOption.value;
      list.options[ii].text  =newOption.text;
      list.options[ii].selected  =newOption.selected;
      }
   if(!selected){ alert("����δѡ��Ҫ�ƶ��ļ�¼����ѡ��"); return false; }
}

//��listFrom��ѡ�м�¼�����м�¼��������listTo��(���listFrom�иü�¼valueΪ�ջ�listTo���Ѿ��иü�¼(��text��ͬ)�����ƶ�)
//listFrom��Ҫ�����ļ�¼���ڵ��б����
//listTo  ����¼Ҫ��������Ŀ���б����
//moveall ���Ƿ񿽱�listFrom�е����м�¼
function copy_list_item(listFrom,listTo,moveall)
{
   var selected=false;
   var tlen=listTo.options.length;
   for(var ii=0; ii<listFrom.options.length; ii++){
      value=listFrom.options[ii].value;
      //��������ƶ����в���δѡ�У�����valueΪ�գ���continue;
      if((!moveall && !listFrom.options[ii].selected)||(value=="")) continue;
      selected=true;
      //���listTo���Ѿ��иü�¼����continue;
      var flag=0;
      for(var jj=0; jj<tlen; jj++){
         if(listFrom.options[ii].text==listTo.options[jj].text){ flag=1; break; }
         }
      if(flag==1) continue;
      //�ƶ��ü�¼
      var newOption = new Option();
      newOption.value = listFrom.options[ii].value
      newOption.text = listFrom.options[ii].text
      listTo.options[listTo.options.length] = newOption;
      }
   if(!selected){ alert("����δѡ���¼����ѡ��"); return false; }
}

//ɾ��list��ѡ�еļ�¼�����м�¼
//list  :Ҫɾ����¼���б����
//delall:�Ƿ�ɾ���б��е����м�¼
function delete_list_item(list,delall,clearflag)
{
  //��Ҫɾ������ı����ֵ������Ϊ��
  for(var ii=0; ii<list.options.length; ii++)  {
    if((delall || list.options[ii].selected) &&list.options[ii].value!="") {
       list.options[ii].value = ""
       list.options[ii].text = ""
       }
    }
  //����б����Ѿ�ɾ������
  var ii=0;jj=0;
  for(;(ii<list.options.length)&&(list.options[ii].text!="");ii++);
  if(ii>=list.options.length){ if(!delall) alert("����ѡ���¼��"); return; }
  for(jj=ii+1;;){
     for(;(jj<list.options.length)&&(list.options[jj].text=="");jj++);
     if(jj>=list.options.length) break;
     list.options[ii].value = list.options[jj].value
     list.options[ii].text = list.options[jj].text
     ii=ii+1;jj=jj+1;
     }
  list.options.length = ii;
  //���ѡ�б�־
  for(var ii=0; ii<list.options.length; ii++){
    if(clearflag) list.options[ii].selected=false;
    }
  }

//��listFrom�����м�¼��text��value�ֱ𱣴浽listText��listValue�в�ѡ�У������Ϳ�����Ϊ�������ݵ���������
//listFrom ��Ҫ������text��value���б����
//listText ������text���ݵ��б����
//listValue������value���ݵ��б����
function save_list_item(listFrom,listText,listValue)
{
   var len = listFrom.length;
   for(var ii=0; ii<len; ii++)  {
      var newOptionText  = new Option("",listFrom.options[ii].text);
      var newOptionValue = new Option("",listFrom.options[ii].value);
      listText.options[ii] = newOptionText;
      listValue.options[ii] = newOptionValue;
      listText.options[ii].selected=true;
      listValue.options[ii].selected=true;
      }
   listText.options.length = len;
   listValue.options.length= len;
}


//�ں�̨ϵͳ��ҳ���е���ѡ���û�/Ⱥ�鹦��(ģ̬����)
//URL  ����ҳ��URL(δ�õ����ò����Ժ�Ҫȡ��)
//user1��Ҫѡ��ĵ�һ���û������룩
//user2��Ҫѡ��ĵ�һ���û������룩
//user3��Ҫѡ��ĵ�һ���û������룩
//stype��Ҫѡ����û�����("":ѡ�񵥸��û�,msg:ѡ��2����Ϣ�û�,task:ѡ��2�������û�,mail:ѡ��3���ʼ��û�,form:ѡ��3�����û�,proj:ѡ��3����Ŀ�û�)
//utype��Ҫѡ�����:""���У�user�û���role��ɫ��dept���ţ�groupȺ��
//alluser���Ƿ���ʾ�����û�(null/""/true����ʾ�����û���false/����:ֻ��ʾδ���õ��û�)



var popup_path="";//����ҳ��popup.apsx������URL

//ѡ����Ա������name
function seladdrname(seltype,username)
{ 
   //seltypeѡ������ user:��Ա.dept:����.role:��ɫ.all:ѡ����Ա���Ž�ɫ single=true��ѡ single=false��ѡ
   //page=selidname ����id,name  page=selname����name
   var url = "address.aspx?page=selname&single=false&seltype="+seltype+"&seltext=" + username;
   revalue = window.showModalDialog(url,'dialog','dialogHeight:500px;dialogWidth:620px;status:no;scroll:yes;resizable:yes;help:no;center:yes;');
   if(revalue != undefined)
   {
      return revalue;
   }
}
      
//�ں�̨ϵͳ��ҳ���е���ѡ���û�/Ⱥ�鹦��(ģ̬����)
//URL  ����ҳ��URL(δ�õ����ò����Ժ�Ҫȡ��)
//user1��Ҫѡ��ĵ�һ���û������룩
//user2��Ҫѡ��ĵ�һ���û������룩
//user3��Ҫѡ��ĵ�һ���û������룩
//stype��Ҫѡ����û�����("":ѡ�񵥸��û�,msg:ѡ��2����Ϣ�û�,task:ѡ��2�������û�,mail:ѡ��3���ʼ��û�,form:ѡ��3�����û�,proj:ѡ��3����Ŀ�û�)
//utype��Ҫѡ�����:""���У�user�û���role��ɫ��dept���ţ�groupȺ��
//alluser���Ƿ���ʾ�����û�(null/""/true����ʾ�����û���false/����:ֻ��ʾδ���õ��û�)
function getSelectUser(URL,user1,user2,user3,stype,utype,alluser)
{
   if(user2==null) user2="";
   if(user3==null) user3="";
   if(stype==null) stype="";
   if(utype==null) utype="";
   if(alluser==null) alluser="";
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   var theurl = URL+"?page=selectuser&stype="+stype+"&utype="+utype+"&user1="+user1+"&user2="+user2+"&user3="+user3+"&alluser="+alluser;
   
   alert(theurl);
   var returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:510px;dialogWidth:700px;edge:raised;help:no;status:no;scroll:yes;');
   return returnValue;
}

//�ں�̨ϵͳ��ҳ���е���ѡ���û�/Ⱥ�鹦��(ģ̬����)
//URL  ����ҳ��URL(δ�õ����ò����Ժ�Ҫȡ��)
//user1��ȱʡֵ
//utype��Ҫѡ�����:""���У�user�û���role��ɫ��dept���ţ�groupȺ��
//alluser���Ƿ���ʾ�����û�(null/""/true����ʾ�����û���false/����:ֻ��ʾδ���õ��û�)
function getSelectOne(URL,user1,utype,alluser)
{
   if(utype==null) utype="";
   if(alluser==null) alluser="";
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   var theurl = URL+"?page=selectone&utype="+utype+"&user1="+user1+"&alluser="+alluser;
   var returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:410px;dialogWidth:600px;edge:raised;help:no;status:no;scroll:yes;');
   return returnValue;
}

//��ʾ�û��б�ѡ��һ���û�(ģ̬����)
//URL  ����ҳ��URL(δ�õ����ò����Ժ�Ҫȡ��)
//defaultvalue��ȱʡ�û�(���ֱ���˳������ʾѡ��ȱʡ�û�)
function getUserList(URL,defaultvalue)
{
   if(defaultvalue==null) defaultvalue="";
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   var theurl = URL+"?page=userlist&default="+defaultvalue;
   var returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:420px;dialogWidth:420px;edge:raised;help:no;status:no;scroll:yes;');
   return returnValue;
}

//��tbox��ʾusertext/uservalue��ָ��������û�/Ⱥ���б�
//usertext ����ѡ����û���/Ⱥ�����б�(��������)
//usertype ����ѡ����û���/Ⱥ��������(��������)(0:�û�,1:��ɫ,2:����)
//uservalue����ѡ����û�id/Ⱥ��id�б�(��������)
//tbox     ����ʾָ�������û�/Ⱥ����б����
//mytype   ����ʾ�û�/Ⱥ��ķ�ʽ(0:��ʾ���У�-1:����Ⱥ�飬-2:�����û���-3:���н�ɫ��-4:���в��ţ�����/,1,2,3,:��ʾָ���Ĳ����û�
function show_user_list(usertext,usertype,uservalue,tbox,mytype)
{
   tbox.options.length = 0;
   var ptype="";
   var no = new Option();
   no.value="";
   no.text ="---------��ѡ��---------";
   tbox.options[tbox.options.length] = no;
   for(var ii=0; ii<usertext.length; ii++)  {
      var idx=mytype.indexOf(","+uservalue[ii]+",");
      if(mytype=="0"||mytype=="-1"&&usertype[ii]!="0"||mytype=="-2"&&usertype[ii]=="0"||mytype=="-3"&&usertype[ii]=="1"||mytype=="-4"&&usertype[ii]=="2"||mytype!="-1"&&mytype!="-2"&&mytype!="-3"&&mytype!="-4"&&usertype[ii]=="0"&&idx>=0){
         if(ptype!=""&&ptype!=usertype[ii]){
            var no = new Option();
            no.value="";
            no.text ="---------��ѡ��---------";
            tbox.options[tbox.options.length] = no;
            }
         ptype=usertype[ii];
         var no = new Option();
         no.value= uservalue[ii];
         no.text = usertext[ii];
         tbox.options[tbox.options.length] = no;
         }
      }
}

//�ں�̨ϵͳ��ҳ���е�����������ѡ������/����&ʱ��(ģ̬����)
//URL  ����ҳ��URL(δ�õ����ò����Ժ�Ҫȡ��)
//defaultValue��ȱʡֵ(���ֱ���˳������ʾѡ��ȱʡֵ)
//minyear ����ݵ���Сֵ
//maxyear ����ݵ����ֵ
//vtype   ��������ʾ��ʽ(0:���ڣ�1:����&ʱ��2:����&ʱ&�֣�3:����&ʱ&��&�룬4:ʱ&��&��)
function getCalendar(URL,defaultValue,minyear,maxyear,vtype)
{
   if(minyear==null)
   {
      minyear="";
   }
   if(maxyear==null)
   {
      maxyear="";
   }
   if(vtype==null)
   {
      vtype="0";
   }
   if(popup_path!=null&&popup_path!="") 
   {
      URL=popup_path;
   }
   
   var theurl = URL+"?page=calendar&default="+defaultValue+"&minyear="+minyear+"&maxyear="+maxyear+"&vtype="+vtype;
   var returnValue ;
   
   if(vtype==4) 
   {
      returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:150px;dialogWidth:320px;edge:raised;help:no;status:no;scroll:yes;');
   }
   else 
   {
      returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:280px;dialogWidth:320px;edge:raised;help:no;status:no;scroll:yes;');
   }
   
   return returnValue;
}

function getCalendar2(URL,defaultValue,minyear,maxyear,vtype)
{
   if(minyear==null) minyear="";
   if(maxyear==null) maxyear="";
   if(vtype==null) vtype="2";
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   
   var theurl = URL+"?page=calendar&default="+defaultValue+"&minyear="+minyear+"&maxyear="+maxyear+"&vtype="+vtype;
   var returnValue ;
   
   if(vtype==4) returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:150px;dialogWidth:320px;edge:raised;help:no;status:no;scroll:yes;');
   else returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:280px;dialogWidth:320px;edge:raised;help:no;status:no;scroll:yes;');
   
   return returnValue;
}

function getCalendar2(URL,defaultValue,minyear,maxyear,vtype)
{
   if(minyear==null) minyear="";
   if(maxyear==null) maxyear="";
   if(vtype==null) vtype="2";
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   var theurl = URL+"?page=calendar&default="+defaultValue+"&minyear="+minyear+"&maxyear="+maxyear+"&vtype="+vtype;
   var returnValue ;
   if(vtype==4) returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:150px;dialogWidth:320px;edge:raised;help:no;status:no;scroll:yes;');
   else returnValue = window.showModalDialog(theurl,'dialog','dialogHeight:280px;dialogWidth:320px;edge:raised;help:no;status:no;scroll:yes;');
   return returnValue;
}


//------------------ʵ���������ܵ�ҳ��(share.do?page=calendar)�õ��ķ���---------------
//��������������&ʱ��(�õ�year,month,date,hour,minute,second�ؼ�)
//vtype����������ʱ��ķ�ʽ(0:����,1:����&ʱ,2:����&ʱ��,3:����&ʱ��,4:ʱ��,5:����)
function get_calendar(vtype)
{
   if(date.value<10) date.value="0"+date.value;
   if(vtype==0) return year.value+"-"+month.value+"-"+date.value;
   else if(vtype==1) return year.value+"-"+month.value+"-"+date.value+" "+hour.value;
   else if(vtype==2) return year.value+"-"+month.value+"-"+date.value+" "+hour.value+":"+minute.value;
   else if(vtype==3) return year.value+"-"+month.value+"-"+date.value+" "+hour.value+":"+minute.value+":"+second.value;
   else if(vtype==4) return hour.value+":"+minute.value+":"+second.value;
   else return year.value+"-"+month.value+"-"+date.value;
}

//��ʾ��������������&ʱ��(�õ�oCalendar,date�ؼ�)
//vtype����������ʱ��ķ�ʽ(0:����,1:����&ʱ,2:����&ʱ��,3:����&ʱ��,4:ʱ��,5:����)
function calendar(vtype)
{
   while(oCalendar.rows.length>2){ myNewRow = oCalendar.deleteRow(); }
   
   theDate = new Date(year.value,month.value-1,1); //����11�գ�month.value��1��ʼ��
   theDate1 = theDate.getDay(); //����1�������ڼ�?
   theDate = new Date(year.value,month.value,1);   //����1��
   theDate2 = theDate.getDay(); //����1�������ڼ�?
   
   var oRow;
   var oCell;
   var count=0;
   
   if(theDate1!=0)
   { 
      //�������1�ղ���������(0)
      oRow = oCalendar.insertRow(); count=count+1;
      oRow.align="center";
      oRow.height=20;
      for(ii=0;ii<theDate1;ii++)
      {
         oCell = oRow.insertCell();  oCell.innerHTML="&nbsp;";
      }
   }
   
   var ii=1;
   while(ii<28 || (theDate+1)%7!=theDate2)
   {
      theDate = new Date(year.value,month.value-1,ii); 
      theDate = theDate.getDay();
      
      if(theDate==0)
      {
         oRow = oCalendar.insertRow(); count=count+1;
         oRow.align="center";
         oRow.height=20;
      }
      
      oCell = oRow.insertCell();
      var temp = new Date();
      var cYear = temp.getYear();
      var cMonth = temp.getMonth()+1;
      var cDate  = temp.getDate();
      var text = "&nbsp;&nbsp;"+ii+"&nbsp;&nbsp;";
      
      if(ii==date.value)
      {
         oCell.bgColor="#00FF00";
      }
      else if(ii==cDate && year.value==cYear && month.value==cMonth) 
      {
         oCell.bgColor="yellow";
      }
      
      var color="blue";
      if(theDate==6 || theDate==0) color="red";
      
      if(vtype==0) 
      {
         str = "<font color="+color+" style='{cursor:hand;}' onclick='javascript:date.value="+ii+";returnValue=get_calendar(0);'>"+text+"</font>";
      }
      else
      {
         str = "<font color="+color+" style='{cursor:hand;}' onclick='javascript:date.value="+ii+";calendar("+vtype+"); ";
         if(ii==date.value) str=str+"returnValue=get_calendar("+vtype+");top.close();";
         str=str+"'>"+text+"</font>";
      }
      oCell.innerHTML=str;
      ii++;
   }
   
   for(ii=1;ii<7-theDate;ii++)
   {
      oCell = oRow.insertCell(); oCell.innerHTML="&nbsp;";
   }
   
   if(count<6)
   { 
      //ÿ�¶�Ӧ����������ʾ6�У���������Ͳ�����У��Ա����е��·�ֻ��5�С�
      oRow = oCalendar.insertRow(); count=count+1;
      oRow.align="center";
      oRow.height=20;
      for(ii=0;ii<7;ii++)
      {
         oCell = oRow.insertCell(); oCell.innerHTML="&nbsp;";
      }
   }
}



//�ں�̨ϵͳ��ҳ���е����ϴ��ļ�(��������)
//URL  ����ҳ��URL(δ�õ����ò����Ժ�Ҫȡ��)
//form  ��Ҫ�ϴ��ļ���form����
//field �������ϴ��ļ������ֶζ���
//dir   ���ϴ��ļ����ĸ�Ŀ¼(admin:��̨ϵͳ�ϴ��ļ���Ŀ¼,app:Ӧ��ģ���ϴ��ļ���Ŀ¼,web:��վǰ̨�ϴ��ļ���Ŀ¼,����:��ʱĿ¼)
//prefix���ϴ��ļ��������ļ���(һ���ǰ�ʱ��ת��Ϊ������Ϊ�ļ���)��ǰ׺��
function getFileUpload(URL,form,field,dir,prefix)
{
   if(dir==null) dir="temp";  //temp,admin,app,web
   if(prefix==null) prefix="upload";
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   var theurl = URL+"?page=fileupload&form="+form+"&field="+field+"&dir="+dir+"&prefix="+prefix;
   window.open(theurl,'_blank','toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=no,resizable=no,left=300,top=300,width=450,height=150');
   return;
}

//�ں�̨ϵͳ��ҳ���е���ѡ���ļ�(��������)
//URL  ����ҳ��URL
//form  ��Ҫѡ���ļ���form����
//field ������ѡ���ļ������ֶζ���
//dir   �����ĸ�Ŀ¼ѡ���ļ�(admin:��̨ϵͳ�ϴ��ļ���Ŀ¼,app:Ӧ��ģ���ϴ��ļ���Ŀ¼,web:��վǰ̨�ϴ��ļ���Ŀ¼,����:��ʱĿ¼)
function getFileSelect(URL,form,field,dir)
{
   if(dir==null) dir="temp";  //temp,admin,app,web
   if(popup_path!=null&&popup_path!="") URL=popup_path;
   var theurl = URL+"?page=fileselect&form="+form+"&field="+field+"&dir="+dir;
   window.open(theurl,'_blank','toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=yes,resizable=yes,left=200,top=200,width=500,height=300');
   return;
}
//--------------------------���ṹ�õ��ķ���----------------------------------
// �ڵ����(�ڵ�ID,���ڵ�ID,name:�ڵ�����,url:�ڵ��URL,title:������ʾ,target:��URL�Ĵ�������,icon:�ڵ�ͼƬ,iconOpen:չ���ڵ��ͼƬ,open:�ڵ��Ƿ�չ��)
function Node(id, pid, name, url, title, target, icon, iconOpen, open) {
   this.id    = id;  //�ڵ�ID    Unique identity number.
   this.pid   = pid; //���ڵ�ID,���Ҹ��ڵ�ʱ,�ò���Ϊ-1 Number refering to the parent node. The value for the root node has to be -1.
   this.name  = name;//�ڵ�����  Text label for the node.
   this.url   = url; //�ڵ��URL Url for the node.
   this.title = title; //������ʾ Title for the node.
   this.target= target;//��URL�Ĵ������� Target for the node.
   this.icon  = icon;  //�ڵ�ͼƬ Image file to use as the icon. Uses default if not specified.
   this.iconOpen = iconOpen; //չ���ڵ��ͼƬ Image file to use as the open icon. Uses default if not specified.
   this._io = open || false; //�ڵ��Ƿ�չ��( is the node open).
   this._is = false; //�Ƿ�ѡ��(is selected ?)
   this._ls = false; //�Ƿ����ֵܽڵ��е����һ��(last sibling ?)
   this._hc = false; //�Ƿ����ӽڵ�(has child?)
   this._ai = 0;  //�ڽڵ������е�indexֵ
this._p;  //���ڵ�Ķ���
};

// ������
function dTree(objName,path) {
   this.config = {
      target           : null, //�����ؼ��ϵ������Ӻ���ʲô������ʾ Target for all the nodes.
      folderLinks      : true, //Ŀ¼�����������ӡ�(���Ϊfalse,��������Ŀ¼�ڵ�ʱ���õ�URL��Ч) Should folders be links.
      useSelection     : false, //ʶ��ڵ�ѡ��״̬(�ø�������ʾ)�� Nodes can be selected(highlighted).
      useCookies       : false, //ʹ��Cookies����סѡ�еĽڵ� The tree uses cookies to rember it's state.
      useLines         : true,  //���ṹ�������������� Tree is drawn with lines.
      useIcons         : true,  //���ṹ��������ͼ�� Tree is drawn with icons.
      useStatusText    : false, //��״̬������ʾURLֻ��ʾ�ڵ����ơ� Displays node names in the statusbar instead of the url.
      closeSameLevel   : false, //ĳ���ڵ���ֻ��һ���ӽڵ���չ��״̬(����ͬʱչ������ӽڵ�) Only one node within a parent can be expanded at the same time. openAll() and closeAll() functions do not work when this is enabled.
      inOrder          : true,  //������Ӹ��ڵ㡣(�����˸ò������������ٶ�) If parent nodes are always added before children, setting this to true speeds up the tree.
      folderTitleOpen  : true   //�Ƿ�����㸸�ڵ�ı����л�Ŀ¼��չ��/�ر�״̬
   }
   if(path==null) path="";
   this.icon = {
      root         : path+'images/dtree/base.gif',
      folder       : path+'images/dtree/folder.gif',
      folderOpen   : path+'images/dtree/folderopen.gif',
      node         : path+'images/dtree/page.gif',
      empty        : path+'images/dtree/empty.gif',
      line         : path+'images/dtree/line.gif',
      join         : path+'images/dtree/join.gif',
      joinBottom   : path+'images/dtree/joinbottom.gif',
      plus         : path+'images/dtree/plus.gif',
      plusBottom   : path+'images/dtree/plusbottom.gif',
      minus        : path+'images/dtree/minus.gif',
      minusBottom  : path+'images/dtree/minusbottom.gif',
      nlPlus       : path+'images/dtree/nolines_plus.gif',
      nlMinus      : path+'images/dtree/nolines_minus.gif'
   };
   this.obj = objName; //���ڵ��и���������Ƶ�[��׺]
   this.aNodes = []; //���нڵ�ļ���
   this.aIndent = [];
   this.root = new Node(-1); //�����ڵ㡣�ڽ�������ʾ�����ĸ��ڵ㶼�Ǹýڵ���ӽڵ㡣
   this.selectedNode = null; //ѡ�еĽڵ�
   this.selectedFound = false;
   this.completed = false;
};

// �����µĽڵ� Adds a new node to the node array
//(�ڵ�ID,���ڵ�ID,name:�ڵ�����,url:�ڵ��URL,title:������ʾ,target:��URL�Ĵ�������,icon:�ڵ�ͼƬ,iconOpen:չ���ڵ��ͼƬ,open:�ڵ��Ƿ�չ��)
dTree.prototype.add = function(id, pid, name, url, title, target, icon, iconOpen, open) {
   this.aNodes[this.aNodes.length] = new Node(id, pid, name, url, title, target, icon, iconOpen, open);
};

//���Ӹ��ڵ㣬�ø��ڵ��idΪ���ַ��� Adds a new node to the node array
//(name:�ڵ�����,url:�ڵ��URL,title:������ʾ,target:��URL�Ĵ�������,icon:�ڵ�ͼƬ,iconOpen:չ���ڵ��ͼƬ,open:�ڵ��Ƿ�չ��)
dTree.prototype.addRoot = function(name, url, title, target, icon, iconOpen, open) {
   this.aNodes[this.aNodes.length] = new Node("", -1, name, url, title, target, icon, iconOpen, open);
};

// �ж�ĳ�ڵ�id�Ƿ��Ѿ�����
dTree.prototype.exist = function(id) {
   for(var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n].id ==id) return true;
      }
   return false;
};

// ���ݽڵ�id���ؽڵ����
dTree.prototype.getNode = function(id) {
   for(var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n].id ==id) return this.aNodes[n];
      }
   return null;
};

// ���ݽڵ�id���ظýڵ��������е�����ֵ
dTree.prototype.getIndex = function(id) {
   for(var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n].id ==id) return n;
      }
   return -1;
};

// չ�����еĽڵ� Open all nodes
dTree.prototype.openAll = function() {
   this.oAll(true);
};
//�ر����еĽڵ� close all nodes
dTree.prototype.closeAll = function() {
   this.oAll(false);
};

//�Ѹ����ڵ�ת��Ϊhtml��ʽ��ʾ���� Outputs the tree to the page
dTree.prototype.toString = function() {
   var str = '<div class="dtree">\n';
   if (document.getElementById) {
      if (this.config.useCookies) this.selectedNode = this.getSelected();
      str += this.addNode(this.root);
   } else str += 'Browser not supported.';
   str += '</div>';
   if (!this.selectedFound) this.selectedNode = null;
   this.completed = true;
   return str;
};

//��pNode�ڵ��µ����ṹת��Ϊhtml��ʽ(������this.node()��������this.node()�����еݹ�����˸�this.addNode()����) Creates the tree structure
dTree.prototype.addNode = function(pNode) {
   var str = '';
   var n=0;
   if(this.config.inOrder) n = pNode._ai;
   for(n; n<this.aNodes.length; n++) {
      if (this.aNodes[n].pid == pNode.id) {
         var cn = this.aNodes[n];
         cn._p = pNode;
         cn._ai = n;
         this.setCS(cn);
         if(!cn.target && this.config.target) cn.target = this.config.target;
         if(cn._hc && !cn._io && this.config.useCookies) cn._io = this.isOpen(cn.id);
         if(!this.config.folderLinks && cn._hc) cn.url = null;
         if(this.config.useSelection && cn.id == this.selectedNode && !this.selectedFound) {
            cn._is = true;
            this.selectedNode = n;
            this.selectedFound = true;
            }
         str += this.node(cn, n);
         if (cn._ls) break;
         }
      }
   return str;
};

//���ݽڵ����(node)������ֵ(nodeId)�����ýڵ��html�ַ���(������this.addNode()����)  Creates the node icon, url and text
dTree.prototype.node = function(node, nodeId) {
   var str = '<div class="dTreeNode">' + this.indent(node, nodeId);
   if(this.config.useIcons) {
      if(!node.icon) node.icon = (this.root.id == node.pid) ? this.icon.root : ((node._hc) ? this.icon.folder : this.icon.node);
      if(!node.iconOpen) node.iconOpen = (node._hc) ? this.icon.folderOpen : this.icon.node;
      if(this.root.id == node.pid) {
         node.icon = this.icon.root;
         node.iconOpen = this.icon.root;
         }
      str += '<img width=18 height=18 align=absbottom '+(node._hc ?'style="{cursor : hand;}" onclick="javascript: ' + this.obj + '.o(' + nodeId + ');"':'')+' border=0 id="i' + this.obj + nodeId + '" src="' + ((node._io) ? node.iconOpen : node.icon) + '" alt="" />';
      }
   if(node.url) {
      str += '<a id="s' + this.obj + nodeId + '" class="' + ((this.config.useSelection) ? ((node._is ? 'nodeSel' : 'node')) : 'node') + '" href="' + node.url + '"';
      if(node.title) str += ' title="' + node.title + '"';
      if(node.target) str += ' target="' + node.target + '"';
      if(this.config.useStatusText) str += ' onmouseover="window.status=\'' + node.name + '\';return true;" onmouseout="window.status=\'\';return true;" ';
      if(((node._hc && this.config.folderLinks) || !node._hc)) str += ' onclick="javascript: '+(this.config.useSelection ?this.obj + '.s(' + nodeId + ');':'') +(node._hc && node.pid != this.root.id?this.obj + '.o(' + nodeId + ',true);':'')+'"';
      str += '>';
      }
   else if ((!this.config.folderLinks || !node.url) && node._hc && node.pid != this.root.id && this.config.folderTitleOpen) str += '<a href="javascript:' + this.obj + '.o(' + nodeId + ');" class="node">';
   str += node.name;
   if (node.url || ((!this.config.folderLinks || !node.url) && node._hc)) str += '</a>';
   str += '</div>';
   if (node._hc) {
      str += '<div id="d' + this.obj + nodeId + '" class="clip" style="display:' + ((this.root.id == node.pid || node._io) ? 'block' : 'none') + ';">';
      str += this.addNode(node);
      str += '</div>';
      }
   this.aIndent.pop();
   return str;
};

// ���ӽڵ�ǰ��+/-��ͼƬ���Լ����ṹ���� Adds the empty and line icons
dTree.prototype.indent = function(node, nodeId) {
   var str = '';
   if(this.root.id != node.pid) {
      for(var n=0; n<this.aIndent.length; n++) str += '<img width=18 height=18 align=absbottom border=0 src="' + ( (this.aIndent[n] == 1 && this.config.useLines) ? this.icon.line : this.icon.empty ) + '" alt="" />';
      (node._ls) ? this.aIndent.push(0) : this.aIndent.push(1);
      //������ӽڵ�
      if(node._hc) {
         str += '<img width=18 height=18 align=absbottom style="{cursor : hand;}" onclick="javascript: ' + this.obj + '.o(' + nodeId + ');" border=0 id="j' + this.obj + nodeId + '" src="';
         if (!this.config.useLines) str += (node._io) ? this.icon.nlMinus : this.icon.nlPlus;
         else str += ( (node._io) ? ((node._ls && this.config.useLines) ? this.icon.minusBottom : this.icon.minus) : ((node._ls && this.config.useLines) ? this.icon.plusBottom : this.icon.plus ) );
         str += '" alt="" />';
         }
      //���û���ӽڵ�
      else str += '<img width=18 height=18 align=absbottom border=0 src="' + ( (this.config.useLines) ? ((node._ls) ? this.icon.joinBottom : this.icon.join ) : this.icon.empty) + '" alt="" />';
   }
   return str;
};

//���ĳ�ڵ�node�Ƿ����ӽڵ㣬�Լ��ýڵ��Ƿ�Ϊ�ֵܽڵ�����һ���� Checks if a node has any children and if it is the last sibling
dTree.prototype.setCS = function(node) {
   var lastId;
   for (var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n].pid == node.id) node._hc = true;
      if (this.aNodes[n].pid == node.pid) lastId = this.aNodes[n].id;
   }
   if (lastId==node.id) node._ls = true;
};

//����ѡ�еĽڵ� Returns the selected node
dTree.prototype.getSelected = function() {
   var sn = this.getCookie('cs' + this.obj);
   return (sn) ? sn : null;
};

//��������ʾѡ�еĽڵ� Highlights the selected node
dTree.prototype.s = function(id) {
   if (!this.config.useSelection) return;
   var cn = this.aNodes[id];
   if (cn._hc && !this.config.folderLinks) return;
   if (this.selectedNode != id) {
      if (this.selectedNode || this.selectedNode==0) {
         eOld = document.getElementById("s" + this.obj + this.selectedNode);
         eOld.className = "node";
      }
      eNew = document.getElementById("s" + this.obj + id);
      eNew.className = "nodeSel";
      this.selectedNode = id;
      if (this.config.useCookies) this.setCookie('cs' + this.obj, cn.id);
   }
};

//�л�Ŀ¼��չ��/�ر�״̬ Toggle Open or close
dTree.prototype.o = function(id,status) {
   var cn = this.aNodes[id];
   if(status==null) status=!cn._io;
   this.nodeStatus(status, id, cn._ls);
   cn._io = status;
   if (this.config.closeSameLevel) this.closeLevel(cn);
   if (this.config.useCookies) this.updateCookie();
};

//չ�����нڵ� Open or close all nodes
dTree.prototype.oAll = function(status) {
   for (var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n]._hc && this.aNodes[n].pid != this.root.id) {
         this.nodeStatus(status, n, this.aNodes[n]._ls)
         this.aNodes[n]._io = status;
      }
   }
   if (this.config.useCookies) this.updateCookie();
};

//����չ���ڵ�(nId:�ڵ����������ֵ��bSelect:�Ƿ�ѡ�У�bFirst:false��ʾnId��IDֵ) Opens the tree to a specific node
dTree.prototype.openTo = function(nId, bSelect, bFirst) {
   if (!bFirst) {
      for (var n=0; n<this.aNodes.length; n++) {
         if (this.aNodes[n].id == nId) {
            nId=n;
            break;
         }
      }
   }
   if(nId>=this.aNodes.length) return;
   var cn=this.aNodes[nId]; //ȡ��ָ���Ľڵ�
   if (cn.pid==this.root.id || !cn._p) return; //����Ƕ����ڵ㣬�򷵻�
   cn._io = true;  //����Ϊչ��״̬
   cn._is = bSelect; //����Ϊ�Ƿ�ѡ��״̬
   if (this.completed && cn._hc) this.nodeStatus(true, cn._ai, cn._ls);
   if (this.completed && bSelect) this.s(cn._ai);
   else if (bSelect) this._sn=cn._ai;
   this.openTo(cn._p._ai, false, true);
};

//���ֹر�ĳһ��ε����нڵ� Closes all nodes on the same level as certain node
dTree.prototype.closeLevel = function(node) {
   for (var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n].pid == node.pid && this.aNodes[n].id != node.id && this.aNodes[n]._hc) {
         this.nodeStatus(false, n, this.aNodes[n]._ls);
         this.aNodes[n]._io = false;
         this.closeAllChildren(this.aNodes[n]);
      }
   }
}

//�ر�ĳ�ڵ��µ����нڵ� Closes all children of a node
dTree.prototype.closeAllChildren = function(node) {
   for (var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n].pid == node.id && this.aNodes[n]._hc) {
         if (this.aNodes[n]._io) this.nodeStatus(false, n, this.aNodes[n]._ls);
         this.aNodes[n]._io = false;
         this.closeAllChildren(this.aNodes[n]);
      }
   }
}

//չ��/�ر�ĳһ���ڵ� Change the status of a node(open or closed)
dTree.prototype.nodeStatus = function(status, id, bottom) {
   eDiv   = document.getElementById('d' + this.obj + id);
   eJoin   = document.getElementById('j' + this.obj + id);
   if (this.config.useIcons) {
      eIcon   = document.getElementById('i' + this.obj + id);
      eIcon.src = (status) ? this.aNodes[id].iconOpen : this.aNodes[id].icon;
   }
   eJoin.src = (this.config.useLines)?
   ((status)?((bottom)?this.icon.minusBottom:this.icon.minus):((bottom)?this.icon.plusBottom:this.icon.plus)):
   ((status)?this.icon.nlMinus:this.icon.nlPlus);
   eDiv.style.display = (status) ? 'block': 'none';
};

//���cookie [Cookie] Clears a cookie
dTree.prototype.clearCookie = function() {
   var now = new Date();
   var yesterday = new Date(now.getTime() - 1000 * 60 * 60 * 24);
   this.setCookie('co'+this.obj, 'cookieValue', yesterday);
   this.setCookie('cs'+this.obj, 'cookieValue', yesterday);
};

//����cookie�е�ĳ��ֵ [Cookie] Sets value in a cookie
dTree.prototype.setCookie = function(cookieName, cookieValue, expires, path, domain, secure) {
   document.cookie =
      escape(cookieName) + '=' + escape(cookieValue)
      + (expires ? '; expires=' + expires.toGMTString() : '')
      + (path ? '; path=' + path : '')
      + (domain ? '; domain=' + domain : '')
      + (secure ? '; secure' : '');
};

//��ȡcookie�е�ĳ��ֵ [Cookie] Gets a value from a cookie
dTree.prototype.getCookie = function(cookieName) {
   var cookieValue = '';
   var posName = document.cookie.indexOf(escape(cookieName) + '=');
   if (posName != -1) {
      var posValue = posName + (escape(cookieName) + '=').length;
      var endPos = document.cookie.indexOf(';', posValue);
      if (endPos != -1) cookieValue = unescape(document.cookie.substring(posValue, endPos));
      else cookieValue = unescape(document.cookie.substring(posValue));
   }
   return (cookieValue);
};

//�ѵ�ǰ�򿪵Ľڵ㱣�浽cookie�У�����ڵ�id��.�ŷָ�  [Cookie] Returns ids of open nodes as a string
dTree.prototype.updateCookie = function() 
{
   var str = '';
   for (var n=0; n<this.aNodes.length; n++) {
      if (this.aNodes[n]._io && this.aNodes[n].pid != this.root.id) {
         if (str) str += '.';
         str += this.aNodes[n].id;
      }
   }
   this.setCookie('co' + this.obj, str);
};

//��cookieֵ���ж�ĳ�ڵ��Ƿ�Ϊչ��״̬ [Cookie] Checks if a node id is in a cookie
dTree.prototype.isOpen = function(id) 
{
   var aOpen = this.getCookie('co' + this.obj).split('.');
   for (var n=0; n<aOpen.length; n++)
      if (aOpen[n] == id) return true;
   return false;
};

//����������֧�������push��pop����,���ó���ʵ�� If Push and pop is not implemented by the browser
if (!Array.prototype.push) 
{
   Array.prototype.push = function array_push() 
   {
      for(var i=0;i<arguments.length;i++)
         this[this.length]=arguments[i];
      return this.length;
   }
};
if (!Array.prototype.pop) 
{
   Array.prototype.pop = function array_pop() 
   {
      lastElement = this[this.length-1];
      this.length = Math.max(this.length-1,0);
      return lastElement;
   }
};


//------------------ʵ��ͼ�����õ��ķ���---------------
var chart_cake_onit=true;
var chart_cake_num=0;
var chart_cake_stop;
//�����״ͼ����
function chart_cake_moveup(iteam,txt,top)
{
   temp=eval(iteam);
   tempat=eval(top);
   temptxt=eval(txt);
   at=parseInt(temp.style.top);
   if(chart_cake_num>=5) temptxt.style.display = "";
   if(at>(tempat-10)&&chart_cake_onit)
   {
      chart_cake_num++;
      temp.style.top=at-2;
      chart_cake_stop=setTimeout("chart_cake_moveup(temp,temptxt,tempat)",10);
   }
}
//�����״ͼ����
function chart_cake_movedown(iteam,txt,top)
{
   temp=eval(iteam);
   temptxt=eval(txt);
   clearTimeout(chart_cake_stop);
   temp.style.top=top;
   chart_cake_num=0;
   temptxt.style.display = "none";
}
//��ԭ�����״ͼ���ƶ�����
function chart_cake_movereset(over)
{
   if(over==1) chart_cake_onit=false;
   else chart_cake_onit=true;
}
//�Ŵ���Сͼ��
function chart_zoom(id,action,zoomXY)
{
   var obj = document.getElementById(id).style;
   var k = 1.11;
   if(action=="-") k = 0.9;
   var width=parseInt(obj.width.replace("px",""))
   var height=parseInt(obj.height.replace("px",""))
   if(zoomXY.match("x")&&(width>=100||k>1)&&(width<=2000||k<1))
   {
      obj.width = width*k;
   }
   if(zoomXY.match("y")&&(height>=100||k>1)&&(height<=2000||k<1))
   {
      obj.height = height*k;
   }
}






/*ҵ��Ӧ�ÿ�������*/

//ȷ���Ƿ�ɾ��ѡ�еļ�¼����ȷ���Ƿ�Ҫִ��ɾ����
function confirmdel()
{
   selected = false;
   for(var ii = 0;ii < frmdg.elements.length;ii++)
   {
      var e = frmdg.elements[ii];
      if(e.type=="checkbox" && e.id!='chkall' && e.id.indexOf('chk')==0 && e.checked)  selected=true;
   }
   if(!selected)
   { 
      alert("��ѡ��Ҫɾ���ļ�¼��"); 
      return false; 
   }
   if(confirm('ȷʵҪɾ����ѡ��¼��'))
   {
      frmdg.action.value='delete'; 
      frmdg.reload.value='reload'; 
      frmdg.submit();
      return true;
   }
   return false;
}

function tosearch()
{
   if(document.getElementById("tblsearch").style.display == "none")
   {
      document.getElementById("tblsearch").style.display = "inline";
   }
   else
   {
      document.getElementById("tblsearch").style.display = "none";
   }
}
function tohsearch()
{
   if(document.getElementById("trhsearch").style.display == "none")
   {
      document.getElementById("trhsearch").style.display = "inline";
      document.getElementById("imgsearch").src = "../skin/images/hsearch.gif";
   }
   else
   {
      document.getElementById("trhsearch").style.display = "none";
      document.getElementById("imgsearch").src = "../skin/images/lsearch.gif";
   }
}

/******************************************************
������	��ʾָ����С���̶���ģʽ�Ի���
���أ�	�Ի���ķ���ֵ
������	strURL �Ի����ļ���ַ
		intWidth �Ի���Ŀ��
		intHeight �Ի���ĸ߶�
		aryParam �Ի���Ĵ������
******************************************************/
function openDialog(strURL, intWidth, intHeight, aryParam)
{
	return window.showModalDialog(strURL,aryParam,'dialogHeight:' + intHeight + 'px;dialogWidth:' + intWidth + 'px;status:no;scroll:yes;resizable:yes;help:no;center:yes;');
}

/******************************************************
������	��ָ����С���̶����´���
���أ�	�Ի���ķ���ֵ
������	strfileName �Ի����ļ���ַ
		intWidth �Ի���Ŀ��
		intHeight �Ի���ĸ߶�
******************************************************/
function openNewWindow(strURL,intWidth, intHeight)
{
	window.open(strURL,'',"toolbar=0,location=0,directories=0,status=1,menubar=0,scrollbars=1,resizable=1,top=0,left=0,width=" + intWidth + ",height=" + intHeight );
}

/******************************************************
������	ȥ���ַ�����ǰ��ո�
���أ�	
������	
******************************************************/
function trimStr(sVal)
{
	var iPos;
	//clear starting space
	while(true)
	{
		iPos = sVal.indexOf(' ');
		if(iPos == -1)
		{
			break;
		}
		
		if(iPos > 0) 
		{
			break;
		}
		sVal = sVal.slice(1);
	}
	//clear ending space
	while(true)
	{
		iPos = sVal.lastIndexOf(' ');
		if(iPos == -1) 
		{
			break;
		}
		
		if(iPos < sVal.length-1)
		{
			break;
		}
	
		sVal = sVal.slice(0, iPos);
	}
	return(sVal);
}

/******************************************************
������	ִ�з������˵ĳ����ļ�
���أ�	�ɹ��򷵻س���ִ�н����ʧ���򷵻�-1
������	strPrgmURL	�����ļ�·��
		   strMethod	���ͷ�����POST��GET��
		   strParamString POST����ʱ�Ĳ����ַ���
******************************************************/
function ExecServerPrgm(strPrgmURL, strMethod, strParamString)
{
	try
	{
		var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
		
		xmlhttp.Open(strMethod, strPrgmURL, false);
		if (strMethod.toUpperCase() == 'POST')
		{
			xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
			xmlhttp.Send(strParamString);			
		}
		else
		{
			xmlhttp.Send();				
		}
		if (xmlhttp.status == 200)
		{
			return unescape(trimStr(xmlhttp.responseText));
		}
		else
		{
			return -1;
		}
	}
	catch(e)
	{
	  return -1;
	}
}

/******************************************************
������	ˢ��ҳ��
���أ�	
������	
******************************************************/
function refreshPage()
{
	window.location.href = window.location.href;
}