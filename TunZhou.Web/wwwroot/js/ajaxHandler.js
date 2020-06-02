/*
 *  ajax请求相关
 */


class AjaxHandler {

    static get(url, success, error) {
        let settings = {
            type: "GET",
            async: true,
            success: function (response) {
                AjaxHandler.apiResponseHandler(response, success);
            }
            //,
            //error: AjaxHandler.createAjaxErrorHandler(error)
        };

        AjaxHandler.ajaxCore(url, settings);
    }

    static post(url, request, success, error, type = true) {
        let settings = {
            type: "POST",
            async: type,
            dataType: "json",
            contentType: "application/json",
            url: url,
            data: JSON.stringify(request),
            success: function (response) {
                AjaxHandler.apiResponseHandler(response, success);
            },
            error: AjaxHandler.createAjaxErrorHandler(error)
        };

        AjaxHandler.ajaxCore(url, settings);
    }

    static postdata(url, request, success, error) {
        let settings = {
            type: "POST",
            async: true,
            dataType: "json",
            contentType: "application/json",
            url: url,
            data: JSON.stringify(request),
            success: function (response) {
                AjaxHandler.apiResponseHandlerdata(response, success);
            },
            error: AjaxHandler.createAjaxErrorHandler(error)
        };

        AjaxHandler.ajaxCore(url, settings);
    }

    static delete(url, success, error) {
        let settings = {
            type: "DELETE",
            async: true,
            success: function (response) {
                AjaxHandler.apiResponseHandler(response, success);
            },
            error: AjaxHandler.createAjaxErrorHandler(error)
        };

        AjaxHandler.ajaxCore(url, settings);
    }

    static createAjaxErrorHandler(error) {
        return error ? error : (xhr, textStatus) => {
            if (!StringHelper.isNullOrWhiteSpace(textStatus)) {
                alert('发生错误：' + textStatus);
            }
        };
    }

    static apiResponseHandler(response, success, error) {
        success(response);
    }

    static apiResponseHandlerdata(response, success, error) {
        success(response);
    }

    static ajaxCore(url, settings) {
        $.ajax(url, settings);
    }

}


