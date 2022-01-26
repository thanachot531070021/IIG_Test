function call(api, method, model, onsucess) {
    $.ajax({
        url: api,
        type: method,
        data: JSON.stringify(model),
        dataType: "json",
        cache: false,
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Content-type", "application/json");
            $('#content_ajaxloader').show();
        },
        success: function (result) {
            if (onsucess) onsucess(result);
        },
        error: function (result) {
            $('#content_ajaxloader').hide();
            $.smallBox({
                title: "เกิดข้อผิดพลาด",
                content: "<i class='fa fa-clock-o'></i> <i>ไม่สามารถทำรายการได้</i>",
                color: "#C46A69",
                iconSmall: "fa fa-times fa-2x fadeInRight animated",
                timeout: 4000,
            });
        }
    });
};



const convert = {
    toBase64: async (f) => {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(f);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        })
    }
}