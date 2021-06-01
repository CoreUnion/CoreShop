/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    /*去掉图片预览框的文字*/
    config.image_previewText = ' ';
    // 界面语言，默认为 'en'
    config.language = 'zh-cn';

    // 设置宽高
    config.height = 500;

    config.toolbarCanCollapse = true;

    //隐藏超链接与高级选项
    config.removeDialogTabs = 'image:advanced;image:Link';

    config.filebrowserHtml5videoUploadUrl = "/Api/Tools/CkEditorUploadFiles";//上传视频的地址

    //上传图片窗口用到的接口
    config.filebrowserImageUploadUrl = "/Api/Tools/CkEditorUploadFiles";
    config.filebrowserUploadUrl = "/Api/Tools/CkEditorUploadFiles";

    // 使上传图片弹窗出现对应的“上传”tab标签
    config.removeDialogTabs = 'image:advanced;link:advanced';

    //粘贴图片时用得到
    config.extraPlugins = 'uploadimage';
    config.uploadUrl = '/Api/Tools/CkEditorUploadFiles';

    // 工具栏（基础'Basic'、全能'Full'、自定义）plugins/toolbar/plugin.js
    config.baseFloatZIndex = 99999999;

};
