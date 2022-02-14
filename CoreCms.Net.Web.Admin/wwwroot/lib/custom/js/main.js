var allWidget = {
    "mediaComponents": [{
        "type": "imgSlide",
        "name": "图片轮播",
        "value": {
            "duration": 2500,
            "list": [{
                "image": "/static/images/common/empty-banner.png",
                "linkType": '',
                "linkValue": ''
            },
            {
                "image": "/static/images/common/empty-banner.png",
                "linkType": '',
                "linkValue": ''
            }
            ]
        },
        "icon": "icon-lunbo"
    },
    //{
    //    "type": "topImgSlide",
    //    "name": "置顶轮播",
    //    "value": {
    //        "duration": 2500,
    //        "list": [{
    //            "image": "/static/images/common/empty-banner.png",
    //            "bg": "/static/images/common/empty-banner.png",
    //            "linkType": '',
    //            "linkValue": ''
    //        },
    //        {
    //            "image": "/static/images/common/empty-banner.png",
    //            "bg": "/static/images/common/empty-banner.png",
    //            "linkType": '',
    //            "linkValue": ''
    //        }
    //        ]
    //    },
    //    "icon": "icon-lunbo"
    //},
    {
        "type": "imgSingle",
        "name": "图片",
        "value": {
            "list": [{
                "image": "/static/images/common/empty-banner.png",
                "linkType": '',
                "linkValue": '',
                "buttonShow": false,
                "buttonText": '',
                "buttonColor": "#FFFFFF",
                "textColor": "#000000"
            }]
        },
        "icon": "icon-zhaopiantubiao"
    },
    {
        "type": "imgWindow",
        "name": "图片分组",
        "value": {
            "style": 2,  // 0 橱窗  2 两列 3三列 4四列
            "margin": 0,
            "list": [
                {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                }, {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "linkType": '',
                    "linkValue": ''
                }
            ]
        },
        "icon": "icon-zidongchuchuang50"
    },
    {
        "type": "video",
        "name": "视频组",
        "value": {
            "autoplay": "false",
            "list": [{
                "image": "/static/images/common/empty-banner.png",
                "url": "",
                "linkType": '',
                "linkValue": ''
            }]
        },
        "icon": "icon-shipin"
    },
    {
        "type": "article",
        "name": "文章组",
        "value": {
            "list": [
                {
                    "title": ''
                }
            ]
        },
        "icon": "icon-wenzhang1"
    },
    {
        "type": "articleClassify",
        "name": "文章分类",
        "value": {
            "limit": 3,
            "articleClassifyId": ''
        },
        "icon": "icon-wenzhangfenlei"
    }
    ],
    "storeComponents": [{
        "type": "search",
        "name": "搜索框",
        "value": {
            "keywords": '请输入关键字搜索',
            "style": 'round' // round:圆弧 radius:圆角 square:方形
        },
        "icon": "icon-sousuokuang"
    },
    {
        "type": "notice",
        "name": "公告组",
        "value": {
            "type": 'auto', //choose手动选择， auto 自动获取
            "list": [
                {
                    "title": "这里是第一条公告的标题",
                    "content": "",
                    "id": ''
                }
            ]
        },
        "icon": "icon-gonggao"
    },
    {
        "type": "navBar",
        "name": "导航组",
        "value": {
            "limit": 4,
            "list": [
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮1",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮2",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮3",
                    "linkType": '',
                    "linkValue": ''
                },
                {
                    "image": "/static/images/common/empty.png",
                    "text": "按钮4",
                    "linkType": '',
                    "linkValue": ''
                }
            ]
        },
        "icon": "icon-daohangliebiao"
    },
    {
        "type": "goods",
        "name": "商品组",
        "icon": "icon-shangpin",
        "value": {
            "title": '商品组名称',
            "lookMore": "true",
            "type": "auto", //auto自动获取  choose 手动选择
            "classifyId": '', //所选分类id
            "brandId": '', //所选品牌id
            "limit": 10,
            "display": "list", //list , slide
            "column": 2, //分裂数量
            "list": [
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                }
            ]
        },
    },
    {
        "type": "goodTabBar",
        "name": "商品选项卡",
        "icon": "icon-shangpin",
        "value": {
            "isFixedHead": "true",//是否固定头部
            "list": [
                {
                    "title": '选项卡名称一',
                    "subTitle": '子标题一',
                    "type": "auto", //auto自动获取  choose 手动选择
                    "classifyId": '', //所选分类id
                    "brandId": '', //所选品牌id
                    "limit": 10,
                    "column": 2, //分裂数量
                    "isShow":true,
                    "list": [
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        }
                    ],
                    "hasChooseGoods": [],
                },
                {
                    "title": '选项卡名称二',
                    "subTitle": '子标题二',
                    "type": "auto", //auto自动获取  choose 手动选择
                    "classifyId": '', //所选分类id
                    "brandId": '', //所选品牌id
                    "limit": 10,
                    "column": 2, //分裂数量
                    "isShow": true,
                    "list": [
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        },
                        {
                            "image": "/static/images/common/empty-banner.png",
                            "name": '',
                            "price": ''
                        }
                    ],
                    "hasChooseGoods": [],
                }
            ]
        },
    },
    {
        "type": "groupPurchase",
        "name": "团购秒杀",
        "value": {
            "title": '活动名称',
            "limit": '10',
            "list": [
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "image": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
            ]
        },
        "icon": "icon-tuangou"
    },
    {
        "type": "pinTuan",
        "name": "拼团",
        "value": {
            "title": '活动名称',
            "limit": '10',
            "list": [
                {
                    "goodsImage": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
                {
                    "goodsImage": "/static/images/common/empty-banner.png",
                    "name": '',
                    "price": ''
                },
            ]
        },
        "icon": "icon-pinTuan"
    },
    {
        "type": "coupon",
        "name": "优惠券组",
        "value": {
            "limit": '2'
        },
        "icon": "icon-tubiao-youhuiquan"
    },
    {
        "type": "service",
        "name": "服务组",
        "value": {
            "title": '推荐服务卡',
            "limit": '10',
            "list": [
                {
                    "thumbnail": "/static/images/common/empty-banner.png",
                    "title": '',
                    "money": ''
                },
                {
                    "thumbnail": "/static/images/common/empty-banner.png",
                    "title": '',
                    "money": ''
                },
            ]
        },
        "icon": "icon-shangpinzu"
    },
    {
        "type": "record",
        "name": "购买记录",
        "value": {
            "style": {
                "top": 20,
                "left": 0
            }
        },
        "icon": "icon-jilu"
    }
    ],
    "utilsComponents": [
        {
            "type": "blank",
            "name": "辅助空白",
            "icon": 'icon-kongbai',
            "value": {
                "height": 20,
                "backgroundColor": "#FFFFFF"
            },
        },
        {
            "type": "textarea",
            "name": "文本域",
            "value": '',
            "icon": 'icon-fuwenben',
        }]
};

var deepClone = function (obj) {
    let result = Array.isArray(obj) ? [] : {}
    for (let key in obj) {
        if (obj.hasOwnProperty(key)) {
            if (typeof obj[key] === 'object') {
                result[key] = deepClone(obj[key]) //递归复制
            } else {
                result[key] = obj[key]
            }
        }
    }
    return result
}
Vue.prototype.bus = new Vue();

Vue.component('layout', {
    template: '#layout',
    name: 'layout',
    data() {
        return {
            pageData: [],
            selectWg: {},
            getPageData: getPageData
        }
    },
    computed: {
        getNumber(val) {
            return function (val) {
                return Number(val)
            }
        }
    },
    mounted() {
        var that = this;
        layui.use(['admin', 'coreHelper'],
            function () {
                var $ = layui.$, coreHelper = layui.coreHelper;
                var test = window.location.href;
                var id = test.split("?id=")[1];
                if (id) {
                    coreHelper.Post(getPageData, { id: id }, function (e) {
                        if (e.code === 0) {
                            that.pageConfig = e.data.pageConfig;
                            if (that.pageConfig.length > 0) {
                                for (var i = 0; i < pageConfig.length; i++) {
                                    var item = pageConfig[i];
                                    var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
                                    item.key = item.type + '_' + elKey
                                }
                                that.pageData = that.pageConfig;
                            }
                        } else {
                            layer.msg(e.msg);
                        }
                    });
                }
            });
    },
    methods: {
        setSelectWg(data) {
            this.selectWg = data
            this.bus.$emit('changeSelectWg', data)
        },
        handleWidgetAdd: function (evt) {
            var newIndex = evt.newIndex;
            var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
            var newObj = deepClone(this.pageData[newIndex])
            newObj.key = this.pageData[newIndex].type + '_' + elKey
            this.$set(this.pageData, newIndex, newObj)
            this.setSelectWg(this.pageData[newIndex])
        },
        handleClickAdd: function (obj) {
            var elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
            var newObj = deepClone(obj)
            newObj.key = obj.type + '_' + elKey;
            var newIndex = this.pageData.length || 0;
            this.$set(this.pageData, newIndex, newObj)
            this.setSelectWg(this.pageData[newIndex])
        },
        handleSelectWidget(index) {
            this.setSelectWg(this.pageData[index])
        },
        handleSelectRecord(index) {
            this.setSelectWg(this.pageData[index])
        },
        deleteWidget(index) {
            if (this.pageData.length - 1 === index) {
                if (index === 0) {
                    this.setSelectWg([])
                } else {
                    this.setSelectWg(this.pageData[index - 1])
                }
            } else {
                this.setSelectWg(this.pageData[index + 1])
            }
            this.$nextTick(() => {
                this.pageData.splice(index, 1)
            })
        },
        handleWidgetDelete(deleteIndex) {
            var that = this;
            layer.open({
                title: '提示',
                content: '确定要删除吗？',
                btn: ['确定', '取消'],
                yes: function (index, layero) {
                    that.deleteWidget(deleteIndex);
                    layer.close(index)
                },
                btn2: function () {
                    return
                }
            });

        },
        handleWidgetClone(index) {
            let cloneData = deepClone(this.pageData[index])
            cloneData.key =
                this.pageData[index].type +
                '_' +
                Date.now() +
                '_' +
                Math.ceil(Math.random() * 1000000)
            this.pageData.splice(index, 0, cloneData)
            this.$nextTick(() => {
                this.setSelectWg(this.pageData[index + 1])
            })
        },
        handleDragRemove: function (evt) {
            this.setSelectWg({});
        },
        datadragEnd: function (evt) {

        }
    }
})
Vue.component('upload-img', {
    template: "#upload-img",
    data: function () {
        return {}
    },
    props: ['index', "item"],
    methods: {
        upload: function () {
            this.$emit('upload-img')
        }
    }
})
Vue.component('upload-topslide-bg-img', {
    template: "#upload-topslide-bg-img",
    data: function () {
        return {}
    },
    props: ['index', "item"],
    methods: {
        upload: function () {
            this.$emit('upload-topslide-bg-img')
        }
    }
})
Vue.component('upload-topslide-img', {
    template: "#upload-topslide-img",
    data: function () {
        return {}
    },
    props: ['index', "item"],
    methods: {
        upload: function () {
            this.$emit('upload-topslide-img')
        }
    }
})
Vue.component('select-link', {
    template: '#select-link',
    props: ['type', 'id'],
    data: function () {
        return {
            linkType: linkType,
            articleTypeList: [],
            linkUrl: this.id || '',
            selectType: this.type ? '' + this.type : Object.keys(linkType)[0]
        }
    },
    watch: {
        type(newVal, oldVal) {
            this.selectType = newVal;
            if (newVal == 1) {
                this.linkUrl = this.id
            }
        }
    },
    mounted() {
        var that = this;
        if (!this.type) {
            this.$emit('update:type', Object.keys(linkType)[0])
        }
        var that = this;
        if (that.articleTypeList.length <= 0) {
            layui.use(['coreHelper'],
                function () {
                    var $ = layui.$, coreHelper = layui.coreHelper;
                    var test = window.location.href;
                    var id = test.split("?id=")[1];
                    if (id) {
                        coreHelper.Post("Api/CoreCmsPages/GetArticleTypes",
                            { id: id },
                            function (e) {
                                if (e.code === 0) {
                                    that.articleTypeList = e.data.articleTypes;
                                } else {
                                    layer.msg(e.msg);
                                }
                            });
                    }
                });
        }
    },
    methods: {
        selectLink: function () {
            this.$emit('choose-link')
        },
        changeSelect: function () {
            this.$emit('update:type', this.selectType)
            this.$emit("update:id", '')
        },
        updateLinkValue: function () {
            this.$emit("update:id", this.linkUrl)
        },
        updateSelect: function () {
            this.$emit("update:id", this.id)
        }
    }
})
Vue.component('layout-config', {
    template: '#layout-config',
    name: 'LayoutConfig',
    data: function () {
        return {
            selectWg: {},
            _editocoverr: null,
            maxSelectGoods: 10, //选择商品最大数量
            maxNoticeNums: 5, //选择公告最多数量
            catList: catList,
            brandList: brandList,
            hasChooseGoods: [],
            hasChooseGroupGoods: [],
            linkType: linkType,
            linkName: '',
            getDesign: getDesign,
            pageCode: pageCode,
            currentItemIndex: '',
            editor: null,
            defaultGoods: [
                {
                    "image": default_banner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": default_banner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": default_banner,
                    "name": '',
                    "price": ''
                },
                {
                    "image": default_banner,
                    "name": '',
                    "price": ''
                }
            ],
            imgWindowStyle: [
                {
                    "title": '1行2个',
                    "value": 2,
                    "image": imgWindowArr[0]
                },
                {
                    "title": '1行3个',
                    "value": 3,
                    "image": imgWindowArr[1]
                },
                {
                    "title": '1行4个',
                    "value": 4,
                    "image": imgWindowArr[2]
                },
                {
                    "title": '1左3右',
                    "value": 0,
                    "image": imgWindowArr[3]
                },
            ]
        }
    },
    watch: {
        selectWg(newVal, oldVal) {
            if (newVal.type == 'textarea') {
                var that = this;
                this.$nextTick(function () {

                    layui.use(['coreHelper'],
                        function () {
                            var $ = layui.$, coreHelper = layui.coreHelper;
                            console.log("selectWg");
                            if (!that.editor) {
                                var Authorization = layui.data(layui.setter.tableName)[layui.setter.request.tokenName];
                                //重点代码 适配器
                                class UploadAdapter {
                                    constructor(loader) {
                                        this.loader = loader;
                                    }
                                    upload() {
                                        return new Promise((resolve, reject) => {
                                            const data = new FormData();
                                            let file = [];
                                            this.loader.file.then(res => {
                                                file = res; //文件流
                                                data.append('upload', file);
                                                $.ajax({
                                                    url: "/Api/Tools/CkEditorUploadFiles",
                                                    type: 'POST',
                                                    data: data,
                                                    dataType: 'json',
                                                    headers: {
                                                        'Authorization': Authorization
                                                    },
                                                    processData: false,
                                                    contentType: false,
                                                    success: function (data) {
                                                        if (data) {
                                                            console.log(data)
                                                            resolve({
                                                                default: data.url //后端返回的参数 【注】返回参数格式是{uploaded:1,default:'http://xxx.com'}
                                                            });
                                                        } else {
                                                            reject(data.msg);
                                                        }

                                                    }
                                                });
                                            })
                                        });
                                    }
                                    abort() {
                                    }
                                }
                                DecoupledEditor
                                    .create(document.querySelector('#container'),
                                        {
                                            language: 'zh-cn',
                                        })
                                    .then(editor => {
                                        editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
                                            return new UploadAdapter(loader);
                                        };
                                        const toolbarContainer = document.querySelector('#toolbar-container');
                                        toolbarContainer.appendChild(editor.ui.view.toolbar.element);
                                        editor.setData(that.selectWg.value);

                                        that.editor = editor;

                                        editor.model.document.on('change:data', () => {
                                            var data = that.editor.getData();
                                            that.selectWg.value = data;
                                            console.log('The data has changed!');
                                        });
                                    })
                                    .catch(error => {
                                        console.error(error);
                                    });
                            }
                        });



                })
            } else {
                if (this.editor) {
                    this.editor.destroy()
                    this.editor = null;
                }
            }
        }
    },
    computed: {
        getSelectWgName: function (type) {
            return function (type) {
                switch (type) {
                    case 'imgSlide':
                        return '图片轮播'
                        break;
                    //case 'topImgSlide':
                    //    return '置顶轮播'
                    //    break;
                    case 'imgSingle':
                        return '图片'
                        break;
                    case 'imgWindow':
                        return '图片分组'
                        break;
                    case 'video':
                        return '视频组'
                        break;
                    case 'article':
                        return '文章组'
                        break;
                    case 'articleClassify':
                        return '文章分类'
                        break;
                    case 'search':
                        return '搜索框'
                        break;
                    case 'notice':
                        return '公告组'
                        break;
                    case 'navBar':
                        return '导航组'
                        break;
                    case 'goods':
                        return '商品组'
                        break;
                    case 'goodTabBar':
                        return '商品选项卡'
                        break;
                    case 'groupPurchase':
                        return '团购秒杀'
                        break;
                    case 'pinTuan':
                        return '拼团'
                        break;
                    case 'service':
                        return '服务组'
                        break;
                    case 'coupon':
                        return '优惠券组'
                        break;
                    case 'record':
                        return '购买记录'
                        break;
                    case 'blank':
                        return '辅助空白'
                        break;
                    case 'textarea':
                        return '文本域'
                        break;
                    default:
                        return '';
                        break;
                }
            }
        }
    },
    mounted() {
        var that = this;

        this.bus.$on('changeSelectWg', function (data) {
            that.selectWg = data

        })

        that.$nextTick(function () {
            //console.log("edit_cover");
            //console.log(that.selectWg);
            //var _editocoverr = UE.getEditor("edit_cover", {
            //    initialFrameWidth: 800,
            //    initialFrameHeight: 300,
            //    zIndex: 19891026,
            //    single: false
            //});
            //that._editocoverr = _editocoverr;
            //that._editocoverr.ready(function () {
            //    that._editocoverr.hide();
            //    that._editocoverr.addListener('beforeInsertImage', function (t, arg) {
            //        var obj = that._editocoverr.queryCommandValue("serverparam");
            //        that.$set(that.selectWg.value.list[obj.index], 'image', arg[0].src)
            //    }.bind(that));
            //})
        })
        layui.use(['admin', 'coreHelper', 'treeTable'],
            function () {
                var $ = layui.$, coreHelper = layui.coreHelper, treeTable = layui.treeTable;
                var test = window.location.href;
                var id = test.split("?id=")[1];
                if (id) {
                    coreHelper.Post(getDesign, { id: id }, function (e) {
                        if (e.code === 0) {
                            //console.log(e);
                            linkType = { "1": "URL链接", "2": "商品", "3": "文章", "4": "文章分类", "5": "智能表单" };
                            that.pageCode = e.data.model.code;
                            that.brandList = e.data.brandList;
                            that.pageConfig = e.data.pageConfig;
                            that.catList = e.data.categories.data;
                            that.articleTypeList = e.data.articleTypes;
                        } else {
                            layer.msg(e.msg);
                        }
                    });
                }

                var table = layui.table;
                //监听文章列表页工具条
                table.on('tool(LAY-app-CoreCmsArticle-ArticleTable-TableBox)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
                    var data = obj.data; //获得当前行数据
                    var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                    var tr = obj.tr; //获得当前行 tr 的DOM对象
                    if (layEvent === 'selectArticle') { //选择
                        if (that.selectWg.type == 'article') {
                            that.$set(that.selectWg.value.list, 0, data)
                        } else {
                            that.$set(that.selectWg.value.list[that.currentItemIndex], 'linkValue', data.id)
                        }
                        layer.closeAll('page');
                    }
                });

                //监听商品列表页工具条
                table.on('tool(LAY-app-CoreCmsGoods-GoodsTable-TableBox)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
                    var data = obj.data; //获得当前行数据
                    var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                    var tr = obj.tr; //获得当前行 tr 的DOM对象

                    if (layEvent === 'selectGoods') { //选择
                        that.$set(that.selectWg.value.list[that.currentItemIndex], 'linkValue', data.id)
                        layer.closeAll('page');
                    }
                });

                // 监听文章分类列表页工具条
                treeTable.on('tool(LAY-app-CoreCmsArticleType-tableBox)', function (obj) {
                    var data = obj.data;
                    var layEvent = obj.event;
                    var tr = obj.tr;
                    if (layEvent === 'selectType') { //选择
                        if (that.selectWg.type == 'articleClassify') {
                            that.selectWg.value.articleClassifyId = data.id
                        } else {
                            that.$set(that.selectWg.value.list[that.currentItemIndex], 'linkValue', data.id)
                        }
                        layer.closeAll('page');
                    }
                });

                // 监听表单列表页工具条
                table.on('tool(LAY-app-CoreCmsForm-FormTable-TableBox)', function (obj) {
                    var data = obj.data;
                    var layEvent = obj.event;
                    var tr = obj.tr;
                    if (layEvent === 'selectform') { //选择
                        that.$set(that.selectWg.value.list[that.currentItemIndex], 'linkValue', data.id)
                        layer.closeAll('page');
                    }
                })
            })
    },
    methods: {
        slectTplStyle: function (item) {
            this.selectWg.value.style = item.value;
        },
        chooseLink: function (index, type) {
            this.currentItemIndex = index;
            this.$set(this.selectWg.value.list[index], 'linkType', type)
            switch (+type) {
                case 2:
                    this.goods_list()
                    break;
                case 3:
                    this.article_list()
                    break;
                case 4:
                    this.articleType_list()
                    break;
                case 5:
                    this.form_list()
                    break;
                default:
                    break;
            }
        },
        form_list: function () {
            //console.log("获取表单列表");
            layui.admin.popup({
                title: '表单列表',
                area: ['800px', '550px'],
                id: 'LAY-app-CoreCmsCommon-GetForms',
                success: function (layero, index) {
                    layui.view(this.id).render('common/getForms', null).done(function () { });
                }
            });
        },

        goods_list: function () {
            //console.log("获取商品列表");
            layui.admin.popup({
                title: '商品列表',
                area: ['800px', '550px'],
                id: 'LAY-app-CoreCmsCommon-GetGood',
                success: function (layero, index) {
                    layui.view(this.id).render('common/getGoodList', null).done(function () { });
                }
            });
        },
        articleType_list: function () {
            //console.log("获取文章分类列表");
            layui.admin.popup({
                title: '文章分类列表',
                area: ['800px', '550px'],
                id: 'LAY-app-CoreCmsCommon-GetArticleTypes',
                success: function (layero, index) {
                    layui.view(this.id).render('common/getArticleTypes', null).done(function () { });
                }
            });
        },

        resetColor: function () {
            this.selectWg.value.backgroundColor = '#FFFFFF'
        },
        handleSlideRemove: function (index) {
            this.selectWg.value.list.splice(index, 1)
        },
        handleAddSlide: function () {
            this.selectWg.value.list.push({
                url: '',
                image: default_banner
            })
        },
        handleAddNav: function () {
            this.selectWg.value.list.push({
                url: '',
                image: default_img,
                text: '按钮文字'
            })
        },
        upImage: function (index, item) {
            var _that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '图片设置',
                    area: ['800px', '300px'],
                    id: 'LAY-app-CoreCmsCommon-GetNoticeIds',
                    success: function (layero, indexChild) {
                        layui.view(this.id).render('common/getUpLoad', null).done(function () {
                            layui.form.on('submit(LAY-app-getUpLoad-submit)',
                                function (data) {
                                    console.log(data);
                                    _that.$set(_that.selectWg.value.list[index], 'image', data.field.imagesUrl)
                                    layer.close(indexChild);
                                });
                        });
                    }
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        layero.contents().find("#LAY-app-getUpLoad-submit").click();
                    }
                });
            });
        },
        upTopSlideBgImage: function (index, item) {
            var _that = this;
            layui.use(['admin', 'coreHelper', 'cropperImg'],
                function () {
                    var $ = layui.$, coreHelper = layui.coreHelper, cropperImg = layui.cropperImg;
                    cropperImg.cropImg({
                        aspectRatio: 809 / 377,
                        imgSrc: _that.selectWg.value.list[index].bg,
                        onCrop: function (data) {
                            var loadIndex = layer.load(2);
                            coreHelper.Post("api/Tools/UploadFilesFByBase64", { base64: data }, function (res) {
                                if (0 === res.code) {
                                    _that.$set(_that.selectWg.value.list[index], 'bg', res.data.fileUrl)
                                    layer.msg(res.msg);
                                    layer.close(loadIndex);
                                } else {
                                    layer.close(loadIndex);
                                    layer.msg(res.msg, { icon: 2, anim: 6 });
                                }
                            });
                        }
                    });
                })
        },
        upTopSlideImage: function (index, item) {
            var _that = this;
            layui.use(['admin', 'coreHelper', 'cropperImg'],
                function () {
                    var $ = layui.$, coreHelper = layui.coreHelper, cropperImg = layui.cropperImg;
                    cropperImg.cropImg({
                        aspectRatio: 880 / 272,
                        imgSrc: _that.selectWg.value.list[index].bg,
                        onCrop: function (data) {
                            var loadIndex = layer.load(2);
                            coreHelper.Post("api/Tools/UploadFilesFByBase64", { base64: data }, function (res) {
                                if (0 === res.code) {
                                    _that.$set(_that.selectWg.value.list[index], 'image', res.data.fileUrl)
                                    layer.msg(res.msg);
                                    layer.close(loadIndex);
                                } else {
                                    layer.close(loadIndex);
                                    layer.msg(res.msg, { icon: 2, anim: 6 });
                                }
                            });
                        }
                    });
                })
        },
        article_list: function () {
            //console.log("获取文章列表");
            layui.admin.popup({
                title: '文章分类列表',
                area: ['800px', '550px'],
                id: 'LAY-app-CoreCmsCommon-GetArticles',
                success: function (layero, index) {
                    layui.view(this.id).render('common/getArticles', null).done(function () {
                        window.box = index;
                    });
                }
            });
        },
        changeGoodsType: function (val) {
            if (val == 'auto') {
                this.hasChooseGoods = this.selectWg.value.list;
                this.selectWg.value.list = this.defaultGoods
            } else {
                this.selectWg.value.list = this.hasChooseGoods.length > 0 ? this.hasChooseGoods : this.defaultGoods
            }
        },
        handleDeleteNotice: function (index) {
            this.selectWg.value.list.splice(index, 1)
        },
        handleDeleteGoods: function (index) {
            this.selectWg.value.list.splice(index, 1)
        },

        //切换TabBar商品来源类型
        changeTabBarGoodsType: function (val, key) {
            console.log(val);
            console.log(key);
            if (val == 'auto') {
                this.selectWg.value.list[key].hasChooseGoods = this.selectWg.value.list[key].list;
                this.selectWg.value.list[key].list = this.defaultGoods
            } else {
                this.selectWg.value.list[key].list = this.selectWg.value.list[key].hasChooseGoods.length > 0 ? this.selectWg.value.list[key].hasChooseGoods : this.defaultGoods
            }
        },
        handleDeleteTabBarGoods: function (key, index) {
            console.log(key);
            console.log(index);
            this.selectWg.value.list[key].list.splice(index, 1)
        },
        handleRemoveTabBar: function (index) {
            this.selectWg.value.list.splice(index, 1)
        },
        handleChange: function (index) {
            console.log(index);
        },
        handleAddTabBarGoods: function () {
            this.selectWg.value.list.push({
                "title": '选项卡名称',
                "subTitle": '子标题',
                "type": "auto", //auto自动获取  choose 手动选择
                "classifyId": '', //所选分类id
                "brandId": '', //所选品牌id
                "limit": 10,
                "column": 2, //分裂数量
                "isShow": false,
                "list": [
                    {
                        "image": "/static/images/common/empty-banner.png",
                        "name": '',
                        "price": ''
                    },
                    {
                        "image": "/static/images/common/empty-banner.png",
                        "name": '',
                        "price": ''
                    },
                    {
                        "image": "/static/images/common/empty-banner.png",
                        "name": '',
                        "price": ''
                    },
                    {
                        "image": "/static/images/common/empty-banner.png",
                        "name": '',
                        "price": ''
                    }
                ],
                "hasChooseGoods": [],
            })
        },


        selectNotice: function () {
            var that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '公告列表',
                    area: ['1200px', '90%'],
                    id: 'LAY-app-CoreCmsCommon-GetNoticeIds',
                    success: function (layero, index) {
                        layui.view(this.id).render('common/getNoticeIds', null).done(function () {
                            layui.form.on('submit(LAY-app-CoreCmsCommon-GetNoticeIds-submit)',
                                function (data) {
                                    //判断个数是否满足
                                    if (Object.getOwnPropertyNames(ids).length > that.maxNoticeNums) {
                                        layer.msg("最多只能选择" + that.maxNoticeNums + "个");
                                        return false;
                                    }
                                    var arr = []
                                    for (let i in ids) {
                                        arr.push(ids[i]);
                                    }
                                    that.$set(that.selectWg.value, 'list', arr)
                                    layer.close(index);
                                });

                        });
                    }
                });
            });
        },
        selectGroupGoods: function () {
            var that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '团购秒杀列表',
                    area: ['1200px', '90%'],
                    id: 'LAY-app-CoreCmsCommon-getGroupIds',
                    success: function (layero, index) {
                        layui.view(this.id).render('common/getGroupIds', null).done(function () {
                            layui.form.on('submit(LAY-app-CoreCmsCommon-GetGroupIds-submit)',
                                function (data) {
                                    //判断个数是否满足
                                    if (Object.getOwnPropertyNames(ids).length > that.maxSelectGoods) {
                                        layer.msg("最多只能选择" + that.maxSelectGoods + "个");
                                        return false;
                                    }
                                    var arr = []
                                    for (let i in ids) {
                                        arr.push(ids[i]);
                                    }

                                    console.log(arr);

                                    that.$set(that.selectWg.value, 'list', arr)
                                    layer.close(index);
                                });
                        });
                    }
                });
            });
        },
        selectPinTuanGoods: function () {
            var that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '拼团列表',
                    area: ['1200px', '90%'],
                    id: 'LAY-app-CoreCmsCommon-getPingTuanIds',
                    success: function (layero, index) {
                        layui.view(this.id).render('common/getPingTuanIds', null).done(function () {
                            layui.form.on('submit(LAY-app-CoreCmsPinTuanRule-tableBox_submit)',
                                function (data) {
                                    //判断个数是否满足
                                    if (Object.getOwnPropertyNames(ids).length > that.maxSelectGoods) {
                                        layer.msg("最多只能选择" + that.maxSelectGoods + "个");
                                        return false;
                                    }
                                    var arr = []
                                    for (let i in ids) {
                                        arr.push(ids[i]);
                                    }
                                    console.log(arr);
                                    that.$set(that.selectWg.value, 'list', arr)

                                    layer.close(index);
                                });

                        });
                    }
                });
            });
        },
        selectServices: function () {
            var that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '服务列表',
                    area: ['1200px', '90%'],
                    id: 'LAY-app-CoreCmsCommon-getServiceIds',
                    success: function (layero, index) {
                        layui.view(this.id).render('common/getServiceIds', null).done(function () {
                            layui.form.on('submit(LAY-app-CoreCmsService-tableBox_submit)',
                                function (data) {
                                    //判断个数是否满足
                                    if (Object.getOwnPropertyNames(ids).length > that.maxSelectGoods) {
                                        layer.msg("最多只能选择" + that.maxSelectGoods + "个");
                                        return false;
                                    }
                                    var arr = []
                                    for (let i in ids) {
                                        arr.push(ids[i]);
                                    }
                                    console.log(arr);
                                    that.$set(that.selectWg.value, 'list', arr)

                                    layer.close(index);
                                });

                        });
                    }
                });
            });
        },
        selectGoods: function () {
            var that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '商品列表',
                    area: ['1200px', '90%'],
                    id: 'LAY-app-CoreCmsCommon-getGoodIds',
                    success: function (layero, index) {
                        layui.view(this.id).render('common/getGoodIds', null).done(function () {
                            layui.form.on('submit(LAY-app-CoreCmsGoods-getData)',
                                function (data) {
                                    //判断个数是否满足
                                    if (Object.getOwnPropertyNames(ids).length > that.maxSelectGoods) {
                                        layer.msg("最多只能选择" + that.maxSelectGoods + "个");
                                        return false;
                                    }
                                    var arr = []
                                    for (let i in ids) {
                                        arr.push(ids[i]);
                                    }
                                    that.hasChooseGoods = arr;
                                    that.$set(that.selectWg.value, 'list', arr)
                                    console.log(arr);
                                    layer.close(index);
                                });

                        });
                    }
                });
            });
        },
        selectTabBarGoods: function (key) {
            var that = this;
            layui.use(['form', 'table'], function () {
                layui.admin.popup({
                    title: '商品列表',
                    area: ['1200px', '90%'],
                    id: 'LAY-app-CoreCmsCommon-getGoodIds',
                    success: function (layero, index) {
                        layui.view(this.id).render('common/getGoodIds', null).done(function () {
                            layui.form.on('submit(LAY-app-CoreCmsGoods-getData)',
                                function (data) {
                                    //判断个数是否满足
                                    if (Object.getOwnPropertyNames(ids).length > that.maxSelectGoods) {
                                        layer.msg("最多只能选择" + that.maxSelectGoods + "个");
                                        return false;
                                    }
                                    var arr = []
                                    for (let i in ids) {
                                        arr.push(ids[i]);
                                    }
                                    //that.hasChooseGoods = arr;
                                    that.$set(that.selectWg.value.list[key], 'list', arr)
                                    console.log(arr);
                                    layer.close(index);
                                });

                        });
                    }
                });
            });
        }
    }
})
new Vue({
    el: '#app',
    data: {

    },
    components: {
        "home": {
            template: "#home",
            data() {
                return {
                    storeComponents: allWidget.storeComponents,
                    utilsComponents: allWidget.utilsComponents,
                    mediaComponents: allWidget.mediaComponents,
                    saveUrl: saveUrl
                }
            },
            methods: {
                selectWidget: function (type) {
                    for (var key in allWidget) {
                        for (var index = 0; index < allWidget[key].length; index++) {
                            var element = allWidget[key][index];
                            if (element.type == type) {
                                this.$refs.layout.handleClickAdd(element)
                            }
                        }
                    }
                }
            },
            mounted() {
                var that = this;
                layui.use(['admin', 'form', 'coreHelper'],
                    function () {
                        var $ = layui.$
                            , form = layui.form
                            , admin = layui.admin
                            , coreHelper = layui.coreHelper;
                        form.on('submit(LAY-app-CoreCmsPages-EditForm-submit)',
                            function (data) {
                                var field = data.field; //获取提交的字段
                                var data = that.$refs.layout.pageData;
                                var datalist = [];
                                for (var i = 0; i < data.length; i++) {
                                    let arr = {};
                                    arr.sType = data[i].type;
                                    if (arr.sType == 'textarea') {
                                        arr.sValue = data[i].value;
                                    } else {
                                        arr.sValue = JSON.stringify(data[i].value)
                                    }
                                    datalist.push(arr);
                                }
                                var pageCode = that.$refs.config.pageCode

                                if (!pageCode) {
                                    layer.msg("编码获取失败");
                                    return false;
                                }
                                //提交 Ajax 成功后，关闭当前弹层并重载表格
                                coreHelper.Post(saveUrl, { pageCode: pageCode, datalist: datalist }, function (e) {
                                    console.log(e)
                                    if (e.code === 0) {
                                        layer.msg(e.msg);
                                    } else {
                                        layer.msg(e.msg);
                                    }
                                });
                            });
                    })
            },
        }
    }
});