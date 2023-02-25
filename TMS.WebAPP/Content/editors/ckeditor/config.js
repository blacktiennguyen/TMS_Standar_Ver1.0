/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
  //  config.toolbar =
//[
 //   ['Source', '-', 'Bold', 'Italic', 'syntaxhighlight']
    //];~/Content/editors/tinymce/plugins/ckfinder/ckfinder.js
    config.filebrowserBrowseUrl = '~/Content/editors/ckeditor/plugins/ckfinder2.3.1/ckfinder.html';
    config.filebrowserImageBrowseUrl = '~/Content/editors/ckeditor/plugins/ckfinder2.3.1/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '~/Content/editors/ckeditor/plugins/ckfinder2.3.1/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '~/Content/editors/ckeditor/plugins/ckfinder2.3.1/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '~/Content/editors/ckeditor/plugins/ckfinder2.3.1/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '~/Content/editors/ckeditor/plugins/ckfinder2.3.1/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
//    FCKConfig.EnterMode = 'br';         // p | div | br
//    FCKConfig.ShiftEnterMode = 'p';   // p | div | br
};
