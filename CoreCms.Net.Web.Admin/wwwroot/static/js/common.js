$(function () {

    var x = 22;
    var y = 20;
    $(".coreshop-upload-img").on('click', function (e) {  //绑定一个鼠标悬停时事件
        alert("打开了");
        //新建一个p标签来存放大图片，this.rel就是获取到a标签的大图片的路径，然后追加到body中 
        $("body").append('<p id="bigimage"><img src="' + this.src + '" alt="" /></p>');
        //改变小图片的透明度为0.5，结合上面的CSS，看起来就象是图片变暗了 
        $(this).stop().fadeTo('slow', 0.5);
        //将鼠标的坐标和声明的x，y做运算并赋值给大图片绝对定位的坐标，然后以fadeIn的效果显示 
        $("#bigimage").css({ top: (e.pageY - y) + 'px', left: (e.pageX + x) + 'px' }).fadeIn('fast');
    }, function () { //鼠标离开后  
        //将变暗的图片复原 
        $(this).stop().fadeTo('slow', 1);
        //移除新增的p标签 
        $("#bigimage").remove();
    });
    $(".coreshop-upload-img").mousemove(function (e) {  //绑定一个鼠标移动的事件
        //将鼠标的坐标和声明的x，y做运算并赋值给大图片绝对定位的坐标，这样大图片就能跟随图片而移动了 
        $("#bigimage").css({ top: (e.pageY - y) + 'px', left: (e.pageX + x) + 'px' });
    });

});