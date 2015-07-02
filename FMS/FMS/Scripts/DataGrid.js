//数据表格的全选函数
function CheckAll() {
    $("#CheckAll").click(function () {
        if ($(this).is(":checked")) {
            $("#SingleCheck input").prop("checked", true); //全选事件
        } else {
            $("#SingleCheck input").prop("checked", false); //取消全选
        }
    });
}

//实现点击行tr 选中此行的checkbox
function CheckedTr() {
    $("#TableData tr #td").bind('click', function () {
        //$(this).bind('click', function() {
        if ($(this).parent("tr").find(":checkbox").prop("checked")) {
            $(this).parent("tr").find(":checkbox").prop("checked", false);
            $(this).parent("tr").css('background-color', '#fff');
        } else {
            $(this).parent("tr").find(":checkbox").prop("checked", true);
            $(this).parent("tr").css('background-color', '#ddd');
        }
        //});
    });
}


