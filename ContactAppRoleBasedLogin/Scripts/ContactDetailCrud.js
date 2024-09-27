$(document).ready(function () {
    $("#contactDetailGrid").jqGrid({
        url: "/ContactDetail/GetContactDetails",
        datatype: "json",
        colNames: ["Id", "Number", "Email"],
        colModel: [{ name: "Id", key: true, hidden: true },
        { name: "Number", editable: true, search: false },
        { name: "Email", editable: true, searchoptions: { sopt: ['eq'] } }],
        height: "250",
        caption: "Contact Details",
        pager: "#pager",
        rowNum: 5,
        rowList: [5, 10, 15],
        sortname: 'id',
        sortorder: 'asc',
        viewrecords: true,
        width: "700",
        gridComplete: function () {
            $("#contactDetailGrid").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true, search: true, refresh: true },
                {
                    //edit
                    url: "/ContactDetail/EditContactDetail",
                    closeAfterEdit: true,
                    width: 600,
                    afterSubmit: function (response, postdata) {
                        var result = JSON.parse(response.responseText);
                        if (result.success) {
                            alert(result.message);
                            return [true];
                        }
                        else {
                            alert(result.message);
                            return [false];
                        }
                    }
                },
                {
                    //add
                    url: "/ContactDetail/AddContactDetail",
                    closeAfterAdd: true,
                    width: 600,
                    afterSubmit: function (response, postdata) {
                        var result = JSON.parse(response.responseText);
                        if (result.success) {
                            alert(result.message);
                            return [true];
                        }
                        else {
                            alert(result.message);
                            return [false];
                        }
                    }
                },
                {
                    //delete
                    url: "/ContactDetail/DeleteContactDetail",
                    afterSubmit: function (response, postdata) {
                        var result = JSON.parse(response.responseText);
                        if (result.success) {
                            alert(result.message);
                            return [true];
                        }
                        else {
                            alert(result.message);
                            return [false];
                        }
                    }
                },
                {
                    //search
                    multipleSearch: false,
                    closeAfterSearch: true
                }
            ); //jqgrid closes here
            //$("#refreshButton").click(function () {
            //    //Clear search filters
            //    $("#contactDetailGrid").jqGrid('setGridParam', { search: false })

            //    //Reload the grid data
            //    $("#contactDetailGrid").jqGrid('setGridParam', {page:1}).trigger('reloadGrid')
            //})
        }
    })
})