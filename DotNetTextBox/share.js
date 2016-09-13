var menuslidetimer=null,cw=document.body.clientWidth,editorid='posteditor';var userAgent=navigator.userAgent.toLowerCase();var is_opera=(userAgent.indexOf('opera')!=-1);var is_saf=((userAgent.indexOf('applewebkit')!=-1)||(navigator.vendor=='Apple Computer, Inc.'));var is_webtv=(userAgent.indexOf('webtv')!=-1);var is_ie=((userAgent.indexOf('msie')!=-1)&&(!is_opera)&&(!is_saf)&&(!is_webtv));var is_moz=((navigator.product=='Gecko')&&(!is_saf));var is_kon=(userAgent.indexOf('konqueror')!=-1);var is_ns=((userAgent.indexOf('compatible')==-1)&&(userAgent.indexOf('mozilla')!=-1)&&(!is_opera)&&(!is_webtv)&&(!is_saf));var is_mac=(userAgent.indexOf('mac')!=-1);function menuContext(obj,state){obj.style.cursor=is_ie?'hand':'pointer';var mode=state=='mouseover'?'hover':'normal';obj.className='editor_button'+mode;var tds=findtags(obj,'td');for(var i=0;i<tds.length;i++){if(tds[i].id==editorid+'_menu'){tds[i].className='editor_menu'+mode;}else if(tds[i].id==editorid+'_colormenu'){tds[i].className='editor_colormenu'+mode;}}}
function arraypop(a){if(typeof a!='object'||!a.length){return null;}else{var response=a[a.length-1];a.length--;return response;}}
function findtags(parentobj,tag){if(typeof parentobj.getElementsByTagName!='undefined'){return parentobj.getElementsByTagName(tag);}
else if(parentobj.all&&parentobj.all.tags){return parentobj.all.tags(tag);}
else{return null;}}
function Popup_Handler(){this.open_steps=2;this.open_fade=false;this.active=false;this.menus=new Array();this.activemenu=null;this.hidden_selects=new Array();this.activate=function(active){this.active=active;}
this.register=function(clickactive,controlkey,noimage){this.menus[controlkey]=new Popup_Menu(clickactive,controlkey,noimage);return this.menus[controlkey];}
this.hide=function(){if(this.activemenu!=null)this.menus[this.activemenu].hide();}}
function Popup_Events(){this.controlobj_show=function(e){doane(e);clearTimeout(this.slidetimer);if(popupmenu.activemenu==null||popupmenu.menus[popupmenu.activemenu].controlkey!=this.id){popupmenu.menus[this.id].show(this,false,popupmenu.menus[this.id].clickactive);}}
this.controlobj_onclick=function(e){doane(e);if(popupmenu.activemenu==null||popupmenu.menus[popupmenu.activemenu].controlkey!=this.id){popupmenu.menus[this.id].show(this,false,popupmenu.menus[this.id].clickactive);}
else{popupmenu.menus[this.id].hide();}}
this.controlobj_onmouseover=function(e){doane(e);popupmenu.menus[this.id].hover(this);}
this.menuoption_onclick_function=function(e){this.ofunc(e);popupmenu.menus[this.controlkey].hide();}
this.menuoption_onclick_link=function(e){popupmenu.menus[this.controlkey].choose(e,this);}
this.menuoption_onmouseover=function(e){this.className='popupmenu_highlight';}
this.menuoption_onmouseout=function(e){this.className='popupmenu_option';}}
popupmenu=new Popup_Handler();popupevents=new Popup_Events();function popupmenu_hide(e){if(e&&e.button&&e.button!=1&&e.type=='click'||(cw==document.body.clientWidth&&e.type=='resize'))return true;else popupmenu.hide();rcmenu.hide();cw=document.body.clientWidth;}
function Popup_Menu(clickactive,controlkey,noimage){this.controlkey=controlkey;this.clickactive=clickactive;this.menuname=this.controlkey.split('.')[0]+'_menu';this.slide_open=(is_opera?false:true);this.open_steps=popupmenu.open_steps;this.init_control=function(noimage){this.controlobj=document.getElementById(this.controlkey);this.controlobj.state=false;if(this.controlobj.firstChild&&(this.controlobj.firstChild.tagName=='TEXTAREA'||this.controlobj.firstChild.tagName=='INPUT')){}else{this.controlobj.unselectable=true;if(!noimage){this.controlobj.style.cursor=is_ie?'hand':'pointer';}
if(clickactive){this.controlobj.onclick=popupevents.controlobj_onclick;}else{this.controlobj.onmouseover=popupevents.controlobj_show;}}}
this.init_control(noimage);this.init_menu=function(){this.menuobj=document.getElementById(this.menuname);if(this.menuobj&&!this.menuobj.initialized){this.menuobj.initialized=true;this.menuobj.onclick=ebygum;this.menuobj.style.position='absolute';if(!this.clickactive){this.menuobj.onmouseover=function(){clearTimeout(menuslidetimer);}
this.menuobj.onmouseout=function(){menuslidetimer=setTimeout("menuhide()",500);}}
this.menuobj.style.zIndex=999;if(is_ie&&!is_mac){this.menuobj.style.filter+="progid:DXImageTransform.Microsoft.shadow(direction=135,color=#CCCCCC,strength=2)";}
this.init_menu_contents();}}
this.init_menu_contents=function(){var tds=findtags(this.menuobj,'td');for(var i=0;i<tds.length;i++){if(tds[i].className=='popupmenu_option'||tds[i].className=='editor_colornormal'){if(is_ie&&!is_mac){tds[i].style.filter+="progid:DXImageTransform.Microsoft.Alpha(opacity=85,finishOpacity=100,style=0)";}
tds[i].style.opacity=0.85;if(tds[i].title&&tds[i].title=='nohighlight'){tds[i].title='';}else{tds[i].controlkey=this.controlkey;if(tds[i].className!='editor_colornormal'){tds[i].onmouseover=popupevents.menuoption_onmouseover;tds[i].onmouseout=popupevents.menuoption_onmouseout;}
if(typeof tds[i].onclick=='function'){tds[i].ofunc=tds[i].onclick;tds[i].onclick=popupevents.menuoption_onclick_function;}else{tds[i].onclick=popupevents.menuoption_onclick_link;}
if(!is_saf&&!is_kon){try{links=findtags(tds[i],'a');for(var j=0;j<links.length;j++){if(typeof links[j].onclick=='undefined')links[j].onclick=ebygum;}}
catch(e){}}}}}}
this.show=function(obj,instant){if(!popupmenu.active){return false;}
else if(!this.menuobj){this.init_menu();}
if(!this.menuobj){return false;}
if(popupmenu.activemenu!=null){popupmenu.menus[popupmenu.activemenu].hide();}
popupmenu.activemenu=this.controlkey;this.menuobj.style.display='';if(popupmenu.slide_open){this.menuobj.style.clip='rect(auto, auto, auto, auto)';}
this.pos=this.fetch_offset(obj);this.leftpx=this.pos['left'];this.toppx=this.pos['top']+obj.offsetHeight;if((this.toppx+this.menuobj.offsetHeight)>=docobj.clientHeight+docobj.scrollTop&&(this.toppx-obj.offsetHeight-this.menuobj.offsetHeight)>0){this.toppx=this.toppx-obj.offsetHeight-this.menuobj.offsetHeight;}if((this.leftpx+this.menuobj.offsetWidth)>=document.body.clientWidth&&(this.leftpx+obj.offsetWidth-this.menuobj.offsetWidth)>0){this.leftpx=this.leftpx+obj.offsetWidth-this.menuobj.offsetWidth;this.direction='right';}else{this.direction='left';}
this.menuobj.style.left=this.leftpx+'px';this.menuobj.style.top=this.toppx+'px';if(!instant&&this.slide_open){this.intervalX=Math.ceil(this.menuobj.offsetWidth/this.open_steps);this.intervalY=Math.ceil(this.menuobj.offsetHeight/this.open_steps);this.slide((this.direction=='left'?0:this.menuobj.offsetWidth),0,0);}else if(this.menuobj.style.clip&&popupmenu.slide_open){this.menuobj.style.clip='rect(auto, auto, auto, auto)';}
this.handle_overlaps(true);}
this.hide=function(e){if(e&&e.button&&e.button!=1){return true;}
this.stop_slide();this.menuobj.style.display='none';this.handle_overlaps(false);popupmenu.activemenu=null;}
this.slidehide=function(){popupmenu.menus[popupmenu.activemenu].hide()}
this.hover=function(obj,clickactive){if(popupmenu.activemenu!=null){if(popupmenu.menus[popupmenu.activemenu].controlkey!=this.id){this.show(obj,true,clickactive);}}}
this.choose=function(e,obj){var links=findtags(obj,'a');if(links[0]){if(is_ie){links[0].click();window.event.cancelBubble=true;}else{if(e.shiftKey){window.open(links[0].href);e.stopPropagation();e.preventDefault();}else{window.location=links[0].href;e.stopPropagation();e.preventDefault();}}
this.hide();}}
this.slide=function(clipX,clipY,opacity){if(this.direction=='left'&&(clipX<this.menuobj.offsetWidth||clipY<this.menuobj.offsetHeight)){if(popupmenu.open_fade&&is_ie){opacity+=10;this.menuobj.filters.item('DXImageTransform.Microsoft.alpha').opacity=opacity;}
clipX+=this.intervalX;clipY+=this.intervalY;this.menuobj.style.clip="rect(auto, "+clipX+"px, "+clipY+"px, auto)";this.slidetimer=setTimeout("popupmenu.menus[popupmenu.activemenu].slide("+clipX+", "+clipY+", "+opacity+");",0);}else if(this.direction=='right'&&(clipX>0||clipY<this.menuobj.offsetHeight)){if(popupmenu.open_fade&&is_ie){opacity+=10;menuobj.filters.item('DXImageTransform.Microsoft.alpha').opacity=opacity;}
clipX-=this.intervalX;clipY+=this.intervalY;this.menuobj.style.clip="rect(auto, "+this.menuobj.offsetWidth+"px, "+clipY+"px, "+clipX+"px)";this.slidetimer=setTimeout("popupmenu.menus[popupmenu.activemenu].slide("+clipX+", "+clipY+", "+opacity+");",0);}else{this.stop_slide();}}
this.stop_slide=function(){clearTimeout(this.slidetimer);this.menuobj.style.clip='rect(auto, auto, auto, auto)';if(popupmenu.open_fade&&is_ie){this.menuobj.filters.item('DXImageTransform.Microsoft.alpha').opacity=100;}}
this.fetch_offset=function(obj){var left_offset=obj.offsetLeft;var top_offset=obj.offsetTop;while((obj=obj.offsetParent)!=null){left_offset+=obj.offsetLeft;top_offset+=obj.offsetTop;}
return{'left':left_offset,'top':top_offset};}
this.overlaps=function(obj,m){var s=new Array();var pos=this.fetch_offset(obj);s['L']=pos['left'];s['T']=pos['top'];s['R']=s['L']+obj.offsetWidth;s['B']=s['T']+obj.offsetHeight;if(s['L']>m['R']||s['R']<m['L']||s['T']>m['B']||s['B']<m['T']){return false;}
return true;}
this.handle_overlaps=function(dohide){if(is_ie){var selects=findtags(document,'select');if(dohide){var menuarea=new Array();menuarea={'L':this.leftpx,'R':this.leftpx+this.menuobj.offsetWidth,'T':this.toppx,'B':this.toppx+this.menuobj.offsetHeight};for(var i=0;i<selects.length;i++){if(this.overlaps(selects[i],menuarea)){var hide=true;var s=selects[i];while(s=s.parentNode){if(s.className=='popupmenu_popup'){hide=false;break;}}}}}else{while(true){var i=arraypop(popupmenu.hidden_selects);if(typeof i=='undefined'||i==null)break;else selects[i].style.visibility='visible';}}}}}
function doane(eventobj){if(!eventobj||is_ie){window.event.returnValue=false;window.event.cancelBubble=true;return window.event;}else{eventobj.stopPropagation();eventobj.preventDefault();return eventobj;}}
function ebygum(eventobj){if(!eventobj||is_ie){window.event.cancelBubble=true;return window.event;}else{if(eventobj.target.type=='submit')eventobj.target.form.submit();eventobj.stopPropagation();return eventobj;}}
function menuregister(clickactive,controlid,noimage,datefield){if(typeof popupmenu=='object'){popupmenu.register(clickactive,controlid,noimage);}}
function menuhide(){if(popupmenu.activemenu!=null){popupmenu.menus[popupmenu.activemenu].slidehide();}}
if(typeof popupmenu=='object'){if(window.attachEvent&&!is_saf){document.attachEvent('onclick',popupmenu_hide);window.attachEvent('onresize',popupmenu_hide);}else if(document.addEventListener&&!is_saf){document.addEventListener('click',popupmenu_hide,false);window.addEventListener('resize',popupmenu_hide,false);}else{window.onclick=popupmenu_hide;window.onresize=popupmenu_hide;}
popupmenu.activate(true);}
function Error(idEdit){if(bMode)return true;alert(warning);idEdit.focus();return false;}
function resize(change,tid){var newheight=parseInt(document.getElementById("dntb_"+tid).offsetHeight,10)+change;var fsico=document.getElementById('fullscreenico_'+tid)?document.getElementById('fullscreenico_'+tid).className:"";if(fsico!="clickcolor"&&newheight>=adjustsize){document.getElementById("dntb_"+tid).height=newheight+'px';}}
function dhLayer(obj,cssclass){this.content=null;this.id="dhlayer";var layer=document.createElement("DIV");this.show=function(w,h,o){layer.id=this.id;layer.className=cssclass;layer.innerHTML=this.content;layer.style.width=w+"px";layer.style.height=h+"px";if(is_opera){layer.style.overflow="auto";}o.appendChild(layer);}
this.hide=function(){layer.style.display="none";}}
var popMenu=new dhLayer(window,"popupmenu_selectcolor"),docobj=(document.documentElement.clientHeight>=document.body.clientHeight)?document.documentElement:document.body;function colordialogmouseout(obj){obj.style.borderColor="";obj.bgColor="";}
function colordialogmouseover(obj){obj.style.borderColor="#0A66EE";obj.bgColor="#EEEEEE";}
function colordialogmousedown(color,type,obj){if(type!='forecolor'){inserObject(document.getElementById('dntb_'+obj).contentWindow,type,color);}else{format(document.getElementById('dntb_'+obj).contentWindow,type,color);}popMenu.hide();}
function changedStatus(idEdit,flag){var i;for(i=1;i<4;i++){document.getElementById(idEdit+i).style.display="none";}
document.getElementById(idEdit+flag).style.display="block";}
function getCurrentDirectory(){var locHref=location.href;var locArray=locHref.split('/');delete locArray[locArray.length-1];var dirTxt=locArray.join('/');return dirTxt;}
var relativestr="";if(PathType=='AbsoluteRoot'){urlpath='http://'+location.host;}else{var r=getCurrentDirectory().replace(urlpath,"");if(r!="")
{for(var i=0;i<r.split('/').length-1;i++)
{relativestr+="../";}}}
function urlchange(ystr){var ys=ystr;var ns=ystr;if(/<img[^>]+src="([^"]+)"[^>]*>/ig.test(ys)||/<a[^>]+href="([^"]+)"[^>]*>/ig.test(ys) || /<embed[^>]+src="([^"]+)"[^>]*>/ig.test(ys)){var myimg=/<img[^>]+src="([^"]+)"[^>]*>/ig;var myfile=/<a[^>]+href="([^"]+)"[^>]*>/ig;var myvideo=/<embed[^>]+src="([^"]+)"[^>]*>/ig;do{var temp=myimg.exec(ns);if(temp==null){temp=myfile.exec(ns);}
if(temp==null){temp=myvideo.exec(ns);}
if(temp==null){break;}
var l_s=temp[0];var needh=l_s;do{var re0=new RegExp(urlpath,'gi');l_s=l_s.replace(re0,''+relativestr);}
while(l_s.indexOf(urlpath)>=0);ys=ys.replace(needh,l_s);}while(1==1);}
return ys;}
function checkformat(name)
{var oSel=document.getElementById("dntb_"+name).contentWindow.document;if(document.getElementById("fontnamelist")){var fs=oSel.queryCommandValue('fontname');if(fs==''&&!is_ie&&window.getComputedStyle){fs=oSel.body.style.fontFamily;}else if(fs==null){fs='';}
if(fs!=document.getElementById("fontnamelist").fontstate){thingy=fs.indexOf(',')>0?fs.substr(0,fs.indexOf(',')):fs;document.getElementById("fontnamelist").innerHTML="<FONT face="+thingy+">"+thingy+"</span>";document.getElementById("fontnamelist").fontstate=fs;}}
if(document.getElementById("fontsizelist")){var ss=oSel.queryCommandValue('fontsize');if(ss==null||ss==''){ss=formatFontsize(oSel.body.style.fontSize);}
if(ss!=document.getElementById("fontsizelist").sizestate){if(document.getElementById("fontsizelist").sizestate==null){document.getElementById("fontsizelist").sizestate='';}
if(ss!=null)
{document.getElementById("fontsizelist").innerHTML=ss;}
else
{document.getElementById("fontsizelist").innerHTML=selfontsize;}
document.getElementById("fontsizelist").sizestate=ss;}}
var contextcontrols=new Array('bold','italic','underline','justifyleft','justifycenter','justifyright','insertorderedlist','insertunorderedlist');for(var i in contextcontrols){var obj=document.getElementById(contextcontrols[i]+"ico"+"_"+name);if(obj){if(oSel.queryCommandState(contextcontrols[i]))
{obj.className="clickcolor";}
else
{obj.className="outcolor";}}}}
function formatFontsize(csssize)
{switch(csssize){case'7.5pt':case'10px':return 1;case'10pt':return 2;case'12pt':return 3;case'14pt':return 4;case'18pt':return 5;case'24pt':return 6;case'36pt':return 7;default:return null;}}
function buttonstate(state,obj)
{if(state=='overcolor'&obj.className=='outcolor')
{obj.className='overcolor';}
if(state=='outcolor'&obj.className!='clickcolor')
{obj.className='outcolor';}
if(state=='clickcolor'&obj.className=='clickcolor')
{obj.className='outcolor';}
else if(state=='clickcolor'&obj.className!='clickcolor')
{obj.className='clickcolor';}}
function ContextMenu(){this.content=null;this.id="rightclickmenu";var menuwin=document.createElement("DIV");this.show=function(w,h,l,t,o){menuwin.id=this.id;menuwin.style.display="block";menuwin.innerHTML=this.content;menuwin.style.width=w;menuwin.style.height=h+"px";menuwin.style.position="absolute";menuwin.style.left=l+"px";menuwin.style.top=t+"px";menuwin.style.zIndex="999";menuwin.oncontextmenu=function(){return false};if(document.getElementById('rightclickmenu')!=null){o.replaceChild(menuwin,document.getElementById('rightclickmenu'));}else{o.appendChild(menuwin);}
var rcmenutable=document.getElementById("rcmenutable");if(rcmenutable)
{if(docobj.clientHeight-t+docobj.scrollTop<rcmenutable.offsetHeight)
{menuwin.style.top=t-rcmenutable.offsetHeight+"px";}
if(rcmenutable.offsetWidth>102)
{menuwin.style.width=rcmenutable.offsetWidth+5+"px";rcmenutable.style.width=rcmenutable.offsetWidth+5+"px";}
else
{rcmenutable.style.width="100px";menuwin.style.width="100px";}
if(docobj.clientWidth-l+docobj.scrollLeft<rcmenutable.offsetWidth)
{menuwin.style.left=l-rcmenutable.offsetWidth+"px";}
menuwin.style.height=rcmenutable.offsetHeight+"px";}}
this.hide=function(){menuwin.style.display="none";}}
var rcmenu=new ContextMenu();function getMenuItem(commandstr,str,img,useful)
{if(useful)
{return"<tr onmouseup='"+commandstr+";rcmenu.hide();' onmouseover=\"this.style.cursor='pointer';this.style.backgroundColor='#1a71e6';this.style.color='#FFFFFF'\" onmouseout=\"this.style.cursor='pointer';this.style.backgroundColor='#FFFFFF';this.style.color='#000000'\"><td align='center' width='25px' height='25px'><img src='"+skin+"img/"+img+".gif'/></td><td unselectable=on>"+eval(str)+"</td></tr>";}
else
{return"<tr onmouseover=\"this.style.cursor='pointer';this.style.backgroundColor='#1a71e6';this.style.color='#FFFFFF'\" onmouseout=\"this.style.cursor='pointer';this.style.backgroundColor='#FFFFFF';this.style.color='#000000'\"><td align='center' style='filter: gray() alpha(opacity=30);opacity:0.3' width='25px' height='25px'><img src='"+skin+"img/"+img+".gif'/></td><td style='filter: gray() alpha(opacity=30);opacity:0.3'>"+eval(str)+"</td></tr>";}}
function getMenuDivItem(commandstr,str)
{var strname=str;if(strname=="symbol"){strname="specialfont";}return"<tr onmouseup='"+commandstr+"' onmouseover=\"this.style.cursor='pointer';this.style.backgroundColor='#1a71e6';this.style.color='#FFFFFF'\" onmouseout=\"this.style.cursor='pointer';this.style.backgroundColor='#FFFFFF';this.style.color='#000000'\"><td align='center' width='25px' height='25px'><img src='"+skin+"img/"+strname+".gif'/></td><td unselectable=on>"+eval(str)+"</td></tr>";}
function addmenu(idEdit)
{var cfgstr=rcmenucfg.toLowerCase().split(',');var obj=document.getElementById('dntb_'+idEdit).contentWindow;var tagname=is_ie?obj.event.srcElement.tagName.toLowerCase():event.target.tagName.toLowerCase();var oSel=GetSelection(obj);if(is_ie?oSel.parentElement!=null:true){if(!is_ie)
{var el=GetElement(oSel.startContainer.childNodes[oSel.startOffset],"A");el=GetElement(oSel.startContainer,"A");}
else
{var el=GetElement(oSel.parentElement(),"A");}}
else
var el=GetElement(oSel.item(0),"A");var menucontent="<table id='rcmenutable' style='BORDER-RIGHT: #999999 1px solid; BORDER-TOP: #999999 1px solid; FONT-SIZE: 12px; BORDER-LEFT: #999999 1px solid; BORDER-BOTTOM: #999999 1px solid;BACKGROUND-COLOR: #ffffff' border='0' cellpadding='0' cellspacing='0'>";for(var i in cfgstr)
{switch(cfgstr[i])
{case'cut':case'copy':case'removeformat':if(is_ie?obj.document.selection.type!='None':obj.getSelection()!=''){menucontent+=getMenuItem("format(dntb_"+idEdit+",\""+cfgstr[i]+"\")",cfgstr[i],cfgstr[i],true);}else{menucontent+=getMenuItem(null,cfgstr[i],cfgstr[i],false);}
break;case'delete':if(is_ie?obj.document.selection.type!='None':obj.getSelection()!=''){menucontent+=getMenuItem("format(dntb_"+idEdit+",\""+cfgstr[i]+"\")","del",cfgstr[i],true);}else{menucontent+=getMenuItem(null,"del",cfgstr[i],false);}
break;case'paste':menucontent+=getMenuItem("pasteContent(dntb_"+idEdit+")",cfgstr[i],cfgstr[i],true);break;case'selectall':case'redo':case'undo':case'print':case'indent':case'outdent':menucontent+=getMenuItem("format(dntb_"+idEdit+",\""+cfgstr[i]+"\")",cfgstr[i],cfgstr[i],true);break;case'hr':menucontent+="<tr><td colspan='2' align='center'><hr style='color: #dddddd; height: 1px; border: 0; background:#dddddd' width='95%'/></td></tr>";break;case'quote':case'code':case'sup':case'sub':if(is_ie?obj.document.selection.type!='None':obj.getSelection()!=''){menucontent+=getMenuItem("inserObject(dntb_"+idEdit+",\""+cfgstr[i]+"\")",cfgstr[i],cfgstr[i],true);}else{menucontent+=getMenuItem(null,cfgstr[i],cfgstr[i],false);}
break;case'excel':case'nowtime':case'pagebreak':menucontent+=getMenuItem("inserObject(dntb_"+idEdit+",\""+cfgstr[i]+"\")",cfgstr[i],cfgstr[i],true);break;case'resettoolbar':menucontent+=getMenuItem("delallcookie()",cfgstr[i],cfgstr[i],true);break;case'getpage':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"collection.aspx\",\"350\",\"85\",\""+idEdit+"_popup_"+cfgstr[i]+"_menu\")",cfgstr[i]);break;case'help':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"onlineHelp.aspx\",\"230\",\"330\",\""+idEdit+"_popup_"+cfgstr[i]+"_menu\")",cfgstr[i]);break;case'link':if(el)
{menucontent+=getMenuItem("insertlink(dntb_"+idEdit+",\"link.aspx\",\"400\",\"138\")","editlink","url",true);}else{menucontent+=getMenuItem("insertlink(dntb_"+idEdit+",\"link.aspx\",\"400\",\"138\")","addlink","url",true);}
break;case'search':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"search.aspx\",\"330\",\"180\",\""+idEdit+"_popup_"+cfgstr[i]+"_menu\")",cfgstr[i]);break;case'symbol':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"specialfont.aspx\",\"385\",\"210\",\""+idEdit+"_popup_specialfont_menu\")",cfgstr[i]);break;case'qq':case'msn':case'icq':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"onlinecode.aspx\",\"200\",\"90\",\""+idEdit+"_popup_"+cfgstr[i]+"_menu\")",cfgstr[i]);break;case'fullscreen':var fs=document.getElementById("fullscreenico_"+idEdit);if(fs.className=="clickcolor")
{fs.className="outcolor";}
else
{fs.className="clickcolor";}
menucontent+=getMenuItem("fullscreen(\""+idEdit+"\")","full_screen",cfgstr[i],true);break;case'inserttable':menucontent+=getMenuItem("inserTables(dntb_"+idEdit+",\"315\",\"250\")",cfgstr[i],"table",true);break;case'insertimage':menucontent+=getMenuItem("inserImg(dntb_"+idEdit+",\"uploadImg.aspx\",\"570\",\"750\")",cfgstr[i],cfgstr[i],true);break;case'insertfile':menucontent+=getMenuItem("inserFile(dntb_"+idEdit+",\"uploadFile.aspx\",\"570\",\"470\")",cfgstr[i],"upload",true);break;case'inserttemplate':menucontent+=getMenuItem("insertTemplate(dntb_"+idEdit+",\"uploadmedia.aspx\",\"570\",\"470\")",cfgstr[i],"template",true);break;case'insertmedia':menucontent+=getMenuItem("inserMedia(dntb_"+idEdit+",\"uploadtemplate.aspx\",\"570\",\"470\")",cfgstr[i],"media",true);break;case'insertdiv':menucontent+=getMenuItem("insertDiv(dntb_"+idEdit+",\"315\",\"280\")",cfgstr[i],"div",true);break;case'emot':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+skin+"emot.aspx\",320,200,\""+idEdit+"_popup_emot_menu\")",cfgstr[i]);break;case'date':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"calendar.aspx\",240,235,\""+idEdit+"_popup_calendar_menu\")",cfgstr[i]);break;case'calculator':menucontent+=getMenuDivItem("getMenuDiv(\""+idEdit+"\",\""+functionstr+"calculator.aspx\",240,210,\""+idEdit+"_popup_calculator_menu\")",cfgstr[i]);break;case'pastetext':case'pasteword':menucontent+=getMenuItem(cfgstr[i]+"(dntb_"+idEdit+")",cfgstr[i]+"_str",cfgstr[i],true);break;case'properties':if(tagname=="img"||tagname=="table"||tagname=="a"||tagname=="div")
{menucontent+=getMenuItem("getProperties(\""+tagname+"\",dntb_"+idEdit+")",cfgstr[i],cfgstr[i],true);}
break;default:break;}}
rcmenu.content=menucontent+"</table>";if(is_ie)
{rcmenu.show("100%",0,obj.event.screenX-window.screenLeft+docobj.scrollLeft,obj.event.screenY-window.screenTop+docobj.scrollTop,document.body);}
else
{var editorobj=document.getElementById('dntb_'+idEdit);rcmenu.show("100%",0,event.pageX+findPosX(editorobj),findPosY(editorobj)+event.pageY,document.body);}}
function getMenuDiv(tid,url,w,h,divid){rcmenu.content="<table id='rcmenutable' style='BORDER-RIGHT: #999999 1px solid; BORDER-TOP: #999999 1px solid; FONT-SIZE: 12px; BORDER-LEFT: #999999 1px solid; BORDER-BOTTOM: #999999 1px solid;BACKGROUND-COLOR: #ffffff' border='0' cellpadding='0' cellspacing='0'><tr><td><iframe width="+w+" height="+h+" scrolling=no frameBorder=0 src='"+url+"?name=dntb_"+tid+"&type="+divid+"'></iframe></td></tr></table>";if(is_ie)
{rcmenu.show(w,h,event.screenX-window.screenLeft+document.documentElement.scrollLeft,event.screenY-window.screenTop+document.documentElement.scrollTop,document.body);}
else
{rcmenu.show(w,h,event.pageX+findPosX(document.body),findPosY(document.body)+event.pageY,document.body);}}
function getProperties(tagname,obj)
{switch(tagname)
{case'img':inserImg(obj,'uploadImg.aspx','570','740');break;case'a':inserFile(obj,'uploadFile.aspx','570','470');break;case'table':inserTables(obj,'315','250');break;case'div':insertDiv(obj,'315','280');break;default:break;}}
function dbclickcheck(obj)
{switch(is_ie?obj.event.srcElement.tagName.toLowerCase():obj.target.tagName.toLowerCase())
{case'img':inserImg(obj,'uploadImg.aspx','570','740');break;case'a':insertlink(obj,'link.aspx','400','138');break;case'table':inserTables(obj,'315','250');break;case'div':insertDiv(obj,'315','280');break;default:break;}}
var w,h,ch,eh,toolheight,statusheight,editorheight;function fullscreen(tid)
{var fs=document.getElementById('fullscreendiv_'+tid);if(document.getElementById("fullscreenico_"+tid).className=="clickcolor")
{window.onscroll=function(){fs.style.top=docobj.scrollTop+"px";fs.style.left=docobj.scrollLeft+"px";}
w=document.getElementById('toolbar_'+tid).clientWidth;h=document.getElementById('toolbar_'+tid).clientHeight;eh=document.getElementById('dntb_'+tid).clientHeight;ch=docobj.clientHeight;document.getElementById('dntb_'+tid).height=parseInt(ch-document.getElementById('toolbarbox_'+tid).clientHeight-document.getElementById('statusbox_'+tid).clientHeight-2)+"px";document.getElementById('toolbar_'+tid).width=is_moz?"100%":docobj.clientWidth+"px";fs.style.position="absolute";fs.style.width="100%";fs.style.height="100%";fs.style.top=docobj.scrollTop+"px";fs.style.left=docobj.scrollLeft+"px";fs.style.zIndex="100";fs.style.overflow="visible";fs.style.backgroundColor='#FFFFFF';window.onresize=function(){checkSize(tid);}}
else
{document.getElementById('dntb_'+tid).height=eh+"px";document.getElementById('toolbar_'+tid).width=w+"px";fs.style.position="";fs.style.width="";fs.style.height="";fs.style.top="";fs.style.left="";fs.style.zIndex="";fs.style.overflow="";fs.style.backgroundColor='';window.onscroll=function(){return true;}
window.onresize=function(){return true;}}}
function checkSize(str)
{if(document.getElementById("fullscreenico_"+str)?document.getElementById("fullscreenico_"+str).className=="clickcolor":false){document.getElementById('toolbar_'+str).width=is_moz?"100%":docobj.clientWidth+"px";document.getElementById('dntb_'+str).height=parseInt(docobj.clientHeight-document.getElementById('toolbarbox_'+str).clientHeight-document.getElementById('statusbox_'+str).clientHeight-2)+"px";}}
function on_ini(){String.prototype.inc=function(s){return this.indexOf(s)>-1?true:false}
if(is_moz){Event.prototype.__defineGetter__("x",function(){return this.clientX+2});Event.prototype.__defineGetter__("y",function(){return this.clientY+2});}}
window.oDel=function(obj){if(obj!=null){obj.parentNode.removeChild(obj)}}
var dragobj={};function onloadfunction(tid)
{on_ini();var o=document.getElementsByTagName("h1");toolheight=document.getElementById('toolbarbox_'+tid).clientHeight;statusheight=document.getElementById('statusbox_'+tid).clientHeight;editorheight=document.getElementById('toolbar_'+tid).clientHeight;for(var i=0;i<o.length;i++){o[i].onmousedown=addevent;}
for(var j=0;j<4;j++){if(GetCookie("dom"+j))
{var domarray=GetCookie("dom"+j).split(",");var brcont=0;for(var k=0;k<domarray.length;k++)
{if(domarray[k]!="br")
{document.getElementById(domarray[k])?document.getElementById("dom"+j).insertBefore(document.getElementById(domarray[k]),null):null;}else{document.getElementById("dom"+j).insertBefore(document.getElementById("dom"+j).getElementsByTagName(domarray[k])[brcont],null);brcont++;}}}}
if(document.getElementById("dom3").getElementsByTagName("h1").length>0)
{var o2=document.getElementById("dom3").getElementsByTagName("h1");var divobj=document.getElementById("dom3").getElementsByTagName("div");for(var m=0;m<o2.length;m++)
{o2[m].childNodes[0].src=functionstr+"img/dragleft.gif";document.getElementById("dom3").childNodes[m].className="drginline";document.getElementById("dom3").childNodes[m].className="sideinline";}
if(is_ie)
{divobj(tid+"_popup_paragraph")?divobj(tid+"_popup_paragraph").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;divobj(tid+"_popup_specialtype")?divobj(tid+"_popup_specialtype").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;divobj(tid+"_popup_fontname")?divobj(tid+"_popup_fontname").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;divobj(tid+"_popup_fontsize")?divobj(tid+"_popup_fontsize").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;}
document.getElementById('dntb_'+tid).height=document.getElementById('toolbar_'+tid).clientHeight-toolheight-statusheight+"px";document.getElementById('dntb_'+tid).height=document.getElementById('editbox_'+tid).clientHeight+2+"px";}
document.getElementById("dom1").getElementsByTagName("h1").length<=0?document.getElementById("dom1").style.width="126px":"";document.getElementById("dom2").getElementsByTagName("h1").length<=0?document.getElementById("dom2").style.width="136px":"";}
function addevent(e){if(dragobj.o!=null)
return false;e=e||event;dragobj.o=this.parentNode;dragobj.xy=getxy(dragobj.o);dragobj.xx=new Array((e.x-dragobj.xy[1]),(e.y-dragobj.xy[0]));dragobj.o.style.width=dragobj.xy[2]+10+"px";dragobj.o.style.height=dragobj.xy[3]+"px";dragobj.o.style.left=(e.x-dragobj.xx[0])+"px";dragobj.o.style.top=(e.y-dragobj.xx[1])+"px";dragobj.o.style.position="absolute";dragobj.o.style.filter="alpha(opacity=60)";dragobj.o.style.opacity="0.6";var om=document.createElement("div");dragobj.otemp=om;om.style.width=dragobj.xy[2]+"px";om.style.height=dragobj.xy[3]+"px";dragobj.otemp.className="divinline";om.style.border="1px dashed #a9a9a9";dragobj.o.parentNode.insertBefore(om,dragobj.o);return false;}
function onmouseupfunction(tid)
{if(dragobj.o!=null){var olddomid=dragobj.o.parentNode.id;var domobjid=dragobj.otemp.parentNode.id;var oldw=dragobj.o.clientWidth;dragobj.o.style.width="auto";dragobj.o.style.height="auto";dragobj.otemp.parentNode.insertBefore(dragobj.o,dragobj.otemp);dragobj.o.style.position="";oDel(dragobj.otemp);var str="",tagname;for(var i=0;i<document.getElementById(domobjid).childNodes.length;i++){if(i>0)str+=",";tagname=document.getElementById(domobjid).childNodes[i].tagName?document.getElementById(domobjid).childNodes[i].tagName:"";if(tagname.toLowerCase()=="br")
{str+="br";}
else
{str+=document.getElementById(domobjid).childNodes[i].id;}}
for(var j=0;j<4;j++){if(GetCookie("dom"+j))
{SetCookie("dom"+j,GetCookie("dom"+j).replace(dragobj.o.id,""),expirehours);}}
SetCookie(domobjid,str,expirehours);var divobj=dragobj.o.getElementsByTagName("div");if(domobjid=="dom3")
{dragobj.o.getElementsByTagName("h1")[0].childNodes[0].src=functionstr+"img/dragleft.gif";dragobj.o.className="sideinline";if(is_ie)
{divobj(tid+"_popup_paragraph")?divobj(tid+"_popup_paragraph").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;divobj(tid+"_popup_specialtype")?divobj(tid+"_popup_specialtype").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;divobj(tid+"_popup_fontname")?divobj(tid+"_popup_fontname").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;divobj(tid+"_popup_fontsize")?divobj(tid+"_popup_fontsize").style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=1)":null;}}
else
{dragobj.o.getElementsByTagName("h1")[0].childNodes[0].src=functionstr+"img/drag.gif";dragobj.o.className="drginline";if(is_ie)
{divobj(tid+"_popup_paragraph")?divobj(tid+"_popup_paragraph").style.filter="":null;divobj(tid+"_popup_specialtype")?divobj(tid+"_popup_specialtype").style.filter="":null;divobj(tid+"_popup_fontname")?divobj(tid+"_popup_fontname").style.filter="":null;divobj(tid+"_popup_fontsize")?divobj(tid+"_popup_fontsize").style.filter="":null;}}
var fsico=document.getElementById('fullscreenico_'+tid)?document.getElementById('fullscreenico_'+tid).className:"";if(fsico!="clickcolor"&&olddomid!=domobjid&&editorheight<document.getElementById('toolbar_'+tid).clientHeight)
{document.getElementById('dntb_'+tid).height=document.getElementById('toolbar_'+tid).clientHeight-toolheight-statusheight+"px";document.getElementById('dntb_'+tid).height=document.getElementById('editbox_'+tid).clientHeight+2+"px";toolheight=document.getElementById('toolbarbox_'+tid).clientHeight;statusheight=document.getElementById('statusbox_'+tid).clientHeight;editorheight=document.getElementById('toolbar_'+tid).clientHeight;}
if(olddomid=="dom1"||olddomid=="dom2")
{document.getElementById(olddomid).getElementsByTagName("h1")[0]?null:document.getElementById(olddomid).style.width=oldw+"px";}
if(domobjid=="dom1"||domobjid=="dom2")
{document.getElementById(domobjid).style.width="auto";}
dragobj.o.style.opacity="";dragobj.o.style.filter="";divobj=null;dragobj={}}}
function SetCookie(sName,sValue,hours){var expire="";if(hours!=null&&hours>0)
{expire=new Date((new Date()).getTime()+hours*3600000);expire="; expires="+expire.toGMTString();}
document.cookie=sName+"="+escape(sValue)+expire;}
function GetCookie(sName){var aCookie=document.cookie.split("; ");for(var i=0;i<aCookie.length;i++){var aCrumb=aCookie[i].split("=");if(sName==aCrumb[0])
return unescape(aCrumb[1]);}}
function delcookie(name)
{var exp=new Date();exp.setTime(exp.getTime()-1);var cval=GetCookie(name);document.cookie=name+"="+cval+"; expires="+exp.toGMTString();}
function delallcookie()
{delcookie("dom0");delcookie("dom1");delcookie("dom2");delcookie("dom3");window.location.reload();}
function getxy(e){var a=new Array();var t=e.offsetTop;var l=e.offsetLeft;var w=e.offsetWidth;var h=e.offsetHeight;while(e=e.offsetParent){t+=e.offsetTop;l+=e.offsetLeft;}
a[0]=t;a[1]=l;a[2]=w;a[3]=h;return a;}
function inner(o,e){var a=getxy(o);if(e.x+docobj.scrollLeft>a[1]&&e.x+docobj.scrollLeft<(a[1]+a[2])&&e.y+docobj.scrollTop>a[0]&&e.y+docobj.scrollTop<(a[0]+a[3])){if(e.y+docobj.scrollTop<(a[0]+a[3]/2))
return 1;else
return 2;}else
return 0;}
function createtmpl(e,elm,tid){for(var i=0;i<domid;i++){if(document.getElementById("m"+i)==null)
continue;if(document.getElementById("m"+i)==dragobj.o)
continue;var b=inner(document.getElementById("m"+i),e);if(b==0)
continue;if(document.getElementById("m"+i).parentNode.id!="dom3")
{if(elm.parentNode.id!="dom3")
{dragobj.otemp.style.width=elm.offsetWidth+"px";dragobj.otemp.style.height=elm.offsetHeight+"px";}
else
{dragobj.otemp.style.width=elm.offsetHeight+"px";dragobj.otemp.style.height=document.getElementById("m"+i).offsetHeight+"px";}}
else
{if(elm.parentNode.id!="dom3")
{dragobj.otemp.style.width=elm.offsetHeight+"px";dragobj.otemp.style.height=elm.offsetWidth+"px";}
else
{dragobj.otemp.style.width=elm.offsetWidth+"px";dragobj.otemp.style.height=elm.offsetHeight+"px";}}
if(b==1){document.getElementById("m"+i).parentNode.insertBefore(dragobj.otemp,document.getElementById("m"+i));}else{if(document.getElementById("m"+i).nextSibling==null){document.getElementById("m"+i).parentNode.appendChild(dragobj.otemp);}else{document.getElementById("m"+i).parentNode.insertBefore(dragobj.otemp,document.getElementById("m"+i).nextSibling);}}
return}
for(var j=0;j<4;j++){if(document.getElementById("dom"+j).innerHTML.inc("div")||document.getElementById("dom"+j).innerHTML.inc("DIV"))
continue;var op=getxy(document.getElementById("dom"+j));if(e.x>(op[1]+10)&&e.x<(op[1]+op[2]-10)){document.getElementById("dom"+j).appendChild(dragobj.otemp);dragobj.otemp.style.width=(op[2]-10)+"px";if(j==3)
{var objh=document.getElementById('toolbar_'+tid).clientHeight-10;dragobj.otemp.style.height=elm.offsetWidth>objh?objh+"px":elm.offsetWidth+"px";}
else
{dragobj.otemp.style.height=op[3]+10+"px";}}}}
function cleanCode(html,type){switch(type.toLowerCase())
{case"span":html=html.replace(/<\/?SPAN[^>]*>/gi,"");break;case"word":html=html.replace(/<\/?SPAN[^>]*>/gi,"");html=html.replace(/<(\w[^>]*)class=([^|>]*)([^>]*)/gi,"<$1$3");html=html.replace(/<(\w[^>]*)style="([^"]*)"([^>]*)/gi,"<$1$3");html=html.replace(/<(\w[^>]*) lang=([^ |>]*)([^>]*)/gi,"<$1$3");html=html.replace(/<\?\?xml[^>]*>/gi,"");html=html.replace(/<\/?\w+:[^>]*>/gi,"");html=html.replace(/&nbsp;/,' ');var re=new RegExp('(<P)([^>]*>.*?)(<\/P>)','gi');html=html.replace(re,'<div$2</div>');break;case"font":html=html.replace(/<\/?FONT[^>]*>/gi,"");break;case"html":html=toText(html);break;case"style":html=html.replace(/<(\w[^>]*)class=([^|>]*)([^>]*)/gi,"<$1$3");html=html.replace(/<(\w[^>]*)style="([^"]*)"([^>]*)/gi,"<$1$3");break;default:break;}
return html;}
function toText(html)
{var tmpDiv=document.createElement("div");tmpDiv.innerHTML=html;var tmpTxt;if(document.all)
{tmpTxt=tmpDiv.innerText;}
else
{tmpTxt=tmpDiv.textContent;}
tmpDiv=null;return tmpTxt;}