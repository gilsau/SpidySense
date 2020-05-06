$(document).ready(function () {

    //Add Url btn click
    $('#btnAddUrl').click(function () {
        var div = $(this).next();
        var input = '<div class="divWebUrl"><table class="table"><tr><th class="text-right">Name </th><td class="text-left"><input type="text" class="form-control urlName" /></td><th class="text-right">Url </th><td class="text-left"><input type="text" class="form-control webUrl" /></td><th class="text-right">Row Selector </th><td class="text-left"><input type="text" class="form-control rowSelector" /></td><td><span class="btn btn-sm btn-info glyphicon glyphicon-remove btnRemUrl"></span></td></tr><tr><td colspan="7" class="text-left"><span class="btn btn-md btn-info btnAddField" style="margin:5px;">Add Field</span></td></tr></table></div>';
        div.prepend(input);

        setTimeout(function () {
            $(div).find('.urlName').select();
        }, 500);
    });

    //Add Field btn click
    $('body').on('click', '.btnAddField', function () {
        var td = $(this).parent();
        var tbl = '<table class="table tblFields"><tr><th class="text-right">Field </th><td class="text-left"><input type="text" class="fieldName form-control" /></td><th class="text-right">Selector </th><td class="text-left"><input type="text" class="fieldSelector form-control" /></td><td><span class="btn btn-sm btn-info glyphicon glyphicon-remove btnRemField"></span></td></tr></table>';
        td.append(tbl);

        setTimeout(function () {
            $(td).find('.fieldName').select();
        }, 500);
    });

    //Remove url btn click
    $('body').on('click', '.btnRemUrl', function () {
        $(this).closest('div').remove();
    }); 

    //Remove field btn click
    $('body').on('click', '.btnRemField', function () {
        $(this).closest('table').remove();
    }); 

    //Save btn click
    $('#btnSave').click(function () {
        var id = $('input[name="Id"]').val();
        var name = $('input[name="Name"]').val();
        var description = $('input[name="Description"]').val();
        var url = $('input[name="Url"]').val();
        var webUrls = [];

        $('.divWebUrl').each(function () {
            var div = $(this);
            var webUrlFields = [];

			//get fields for webUrl
            $(div).find('.tblFields').each(function () {
                var tbl = $(this);

                webUrlFields.push(
                    {
                        Name: $(tbl).find('.fieldName').val(),
                        Selector: $(tbl).find('.fieldSelector').val()
                    });
            });

			//add WebUrl to array
            var webUrl = {
                Name: $(div).find('.urlName').val(),
                Url: $(div).find('.webUrl').val(),
                RowSelector: $(div).find('.rowSelector').val(),
                WebUrlFields: webUrlFields
            };
            webUrls.push(webUrl);
        });

        var data = {
            Id: id,
            Name: name, 
            Description: description,
            Url: url,
            WebUrls: webUrls
        };

        $.ajax({
            method: "POST",
            url: mvcControllerUrl,
            data: data
        })
            .done(function (msg) {
                window.location = "/websites/";
            });
    });

	//set focus to first textbox
    $('#Name').focus();
});
