(function ($) {

    function GeoRecordIndex() {
        var $this = this, grid, formAddEdit, formImportPresenter;

        function initializeModalWithForm() {
            $(document).off("change", "#SelectedDistrict").on("change", "#SelectedDistrict", function () { 
                bindBlock();
            });
            $(document).off("change", "#SelectedBlock").on("change", "#SelectedBlock", function () {
                bindGP()
            }); 
            $(document).off("click", "#downloadKML").on("click", "#downloadKML", function () {
                var table = $('#grid-records').DataTable(); // Replace '#example' with your table's selector

                if (table.data().count() > 0) {
                    $('.dynamicLinkSlotAddAppointment').remove();
                    var isListDummy = '0';
                    $('body').append('<a id="linkAppointmentAdd" class="dynamicLinkSlotAddAppointment" href=' + domain + 'map/ExportKml?DistrictName=' + $('#SelectedDistrict').val() + '&BlockName=' + $('#SelectedBlock').val() + '&GPName=' + $('#SelectedGP').val() +'>& nbsp;</a >');
                    $('#linkAppointmentAdd')[0].click();               
                } else {
                    Global.Alert("Please select district/block/GP;")
                }
            }); 
           
            function bindBlock() {
                $('#SelectedBlock').empty();
                var districtId = $('#SelectedDistrict').val();
                $('#SelectedBlock').append($('<option/>', {
                    value: null,
                    text: "Select Block"
                }));
                if (districtId != null && districtId != '' && districtId != 'select') {
                    $.get(domain + 'georecord/bindblock?DistrictId=' + districtId, function (result) {

                        if (result != null && !jQuery.isEmptyObject(result)) {

                            $.each(result, function (index, item) {
                                $('#SelectedBlock').append($('<option/>', {
                                    value: item.Value,
                                    text: item.Text
                                }));
                            });
                        };

                    });
                }
            }

            function bindGP() {
                $('#SelectedGP').empty();
                var districtId = $('#SelectedDistrict').val();
                var blockId = $('#SelectedBlock').val();
                $('#SelectedGP').append($('<option/>', {
                    value: null,
                    text: "Select GP"
                }));
                if (districtId != null && districtId != '' && districtId != 'Select District') {
                    $.get(domain + 'georecord/bindgp?DistrictId=' + districtId + '&BlockId=' + blockId, function (result) {
                        if (result != null && !jQuery.isEmptyObject(result)) {
                            $.each(result, function (index, item) {
                                $('#SelectedGP').append($('<option/>', {
                                    value: item.Value,
                                    text: item.Text
                                }));
                            });
                        };
                    });
                }
            }

            $(document).off("click", "#loadMapBtn").on("click", "#loadMapBtn", function () {
                if ($('#SelectedBlock').val() == undefined || $('#SelectedBlock').val() == null || $('#SelectedBlock').val() == 'Select Block' ||
                    $('#SelectedDistrict').val() == undefined || $('#SelectedDistrict').val() == null || $('#SelectedDistrict').val() == '' || $('#SelectedDistrict').val() == 'Select District') {
                    Global.Alert("Please select District and Block to load map");
                    return false;

                }
                initializeGrid();
                
            });

           



            $("#modal-delete-record").on('shown.bs.modal', function (e) {
                var formDeleteJobTitle = new Global.FormHelper($(this).find("form"), { updateTargetId: "validation-summary" }, function (data) {
                    debugger;
                    if (data.isSuccess) {
                        window.location.reload();
                    }
                });
            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });


            $("#modal-import-record").on('shown.bs.modal', function (e) {

                formAddEditPresenter = new Global.FormHelperWithFiles($("#formImportRecord"),
                    {
                        updateTargetId: "validation-summary", beforeSubmit: function () {

                            if ($('#ImportedFile').val() == undefined || $('#ImportedFile').val() == null || $('#ImportedFile').val() == '') {
                                $('.errorfile').html('*required');
                            }
                            if ($('.errorfile').html() == '') { return true; } else { return false; }

                        }
                    }, function onSuccess(result) {
                        if (result) {

                            if (result.message != undefined && result.message != null && result.message != '') {
                                var url = result.message.replace('~', '');
                                var link = document.createElement('a');
                                document.body.appendChild(link);
                                link.href = domain + url;
                                link.click();

                            }
                            setTimeout(function () { window.location.href = result.redirectUrl; }, 200);
                            //;
                        }
                    });

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });

            $(document).off("click", ".alphabet").on("click", ".alphabet", function () {
                $(".alphabet").each(function () {
                    $(this).removeClass("active");
                });
                $(this).addClass("active");
                $("#hdnAlphabet").val($(this).val());

                $objectSearch = {
                    SystemValue: $('.rdSystemValue:checked').map(function () { return this.value; }).get().join(','),
                    AlphabetValue: $("#hdnAlphabet").val()
                }
                initializeGrid();
            })

            $(document).off("click", "#searchProduct").on("click", "#searchProduct", function () {
                $objectSearch = {
                    SystemValue: $('.rdSystemValue:checked').map(function () { return this.value; }).get().join(',')
                }
                initializeGrid();
            });

            $(document).off("click", "#clearSearchProduct").on("click", "#clearSearchProduct", function () {

                $(".rdSystemValue").prop('checked', false);
                $(".alphabet").removeClass("active");
                $("#hdnAlphabet").val('');

                $objectSearch = [];
                initializeGrid();
            });
            $(document).off("click", "#btnExporttoexcel").on("click", "#btnExporttoexcel", function () {

                $('.buttons-excel').trigger('click');
            });
        }
        function downloadFile(urlToSend) {
            var req = new XMLHttpRequest();
            req.open("GET", urlToSend, true);
            req.responseType = "blob";
            req.onload = function (event) {
                var blob = req.response;
                var fileName = req.getResponseHeader("fileName") //if you have the fileName header available
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = fileName;
                link.click();
            };

            req.send();
        }

        function initializeGrid() {
            $objectSearch = null;
            if ($.fn.DataTable.isDataTable($this.gridrecords)) {
                $($this.gridrecords).DataTable().destroy();
            }
            $this.gridrecords = new Global.GridHelper('#grid-records', {
                "columnDefs": [
                    {
                        "targets": [0],
                        "visible": false,
                        "searchable": false
                    },
                    {
                        "targets": [1],
                        "visible": true,
                        "sortable": true,
                        "searchable": true

                    },
                    {
                        "targets": [2],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    },
                    {
                        "targets": [3],
                        "visible": true,
                        "sortable": true,
                        "searchable": true

                    },
                    {
                        "targets": [4],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    }, {
                        "targets": [5],
                        "visible": true,
                        "sortable": true,
                        "searchable": true

                    },
                    {
                        "targets": [6],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    },
                    {
                        "targets": [7],
                        "visible": true,
                        "sortable": true,
                        "searchable": true

                    },
                    {
                        "targets": [8],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    }, {
                        "targets": [9],
                        "visible": true,
                        "sortable": true,
                        "searchable": true

                    },
                    {
                        "targets": [10],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    },
                    {
                        "targets": [11],
                        "visible": true,
                        "sortable": true,
                        "searchable": true

                    },
                    {
                        "targets": [12],
                        "visible": false,
                        "sortable": true,
                        "searchable": true
                    }, {
                        "targets": [13],
                        "visible": true,
                        "sortable": false,
                        "searchable": true

                    },
                    {
                        "targets": [14],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    },
                    {
                        "targets": [15],
                        "visible": true,
                        "sortable": false,
                        "searchable": true

                    },
                    {
                        "targets": [16],
                        "visible": false,
                        "sortable": true,
                        "searchable": true
                    }, {
                        "targets": [17],
                        "visible": true,
                        "sortable": false,
                        "searchable": true

                    },
                    {
                        "targets": [18],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    },
                    {
                        "targets": [19],
                        "visible": true,
                        "sortable": false,
                        "searchable": true

                    },
                  
                    {
                        "targets": [23],
                        "data":[25],
                        "visible": true,
                        "sortable": true,
                        "searchable": true
                    },
                    {
                        "targets": [24],
                        "data": [0],
                        "visible": true,
                        "sortable": true,
                        "searchable": true,
                        "render": function (data, type, row, meta) {
                            var actionLink = ''                         
                            actionLink += $("<a/>", {
                                href: domain + "GeoRecord/DeleteRecord?id=" + row[0],
                                id: "Deletedoctor",
                                class: "action-btn gray",
                                'title': "Delete",
                                'data-toggle': "modal",
                                'data-target': "#modal-delete-record",
                                html: $("<i/>", {
                                    class: "ace-icon fa fa-trash bigger-120"
                                }),
                            }).get(0).outerHTML;



                            return actionLink;
                        }

                    }


                ],
                "direction": "rtl",
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "bProcessing": true,
                "bServerSide": true,
                "bAutoWidth": false,
                "stateSave": false,
                "searching": false,
                 "sLength":true,
                "pageLength": 20, // default number of rows to show               
                "dom": 'Bfrtip', // Define the position of the buttons and table controls
                "buttons": [
                    {
                        extend: 'excelHtml5',
                        id:'btn-exporttoexcel',
                        text: 'Export to Excel',
                        className: 'btn btn-success hidden'
                    }
                ],
                "sAjaxSource": domain + "/GeoRecord/Index",
                "fnServerData": function (url, data, callback) {
                   
                    data.push(
                        { name: "SelectedDistrict", value: $("#SelectedDistrict").val() },
                        { name: "SelectedBlock", value: $("#SelectedBlock").val() }, 
                        { name: "SelectedGP", value: $("#SelectedGP").val() }, 
                         { name: "ShowAll", value: $("#showAll").is(':checked')} 
                    );

                    $.ajax({
                        "url": url,
                        "data": data,
                        "success": callback,
                        "contentType": "application/x-www-form-urlencoded; charset=utf-8",
                        "dataType": "json",
                        "type": "POST",
                        "cache": false,
                        "error": function () {

                        }
                    });
                },
                "fnDrawCallback": function (oSettings) {
                    initGridControlsWithEvents();

                    if (oSettings._iDisplayLength > oSettings.fnRecordsDisplay()) {
                        $(oSettings.nTableWrapper).find('.dataTables_paginate').hide();
                    }
                    else {
                        $(oSettings.nTableWrapper).find('.dataTables_paginate').show();
                        window.scrollTo(0, 0);
                    }
                },

                "stateSaveCallback": function (settings, data) {
                    localStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data))
                },
                "stateLoadCallback": function (settings) {
                    return JSON.parse(localStorage.getItem('DataTables_' + settings.sInstance))
                }

            });
            table = $this.gridrecords.DataTable();
            $('.dataTable').on('draw.dt', function () {
                //bindEnterEventInDataTableJSGrid(true);
            });
            $('.dataTables_filter').css("float", "right");
        }
        function initGridControlsWithEvents() {
            if ($('.switchBox').data('bootstrapSwitch')) {
                $('.switchBox').off('switchChange.bootstrapSwitch');
                $('.switchBox').bootstrapSwitch('destroy');
            }

            $('.switchBox').bootstrapSwitch()
                .on('switchChange.bootstrapSwitch', function () {
                    var switchElement = this;
                    $.post(domain + 'Admin/Allergies/UpdateStatus', { id: this.value, isActive: !this.checked },
                        function (result) {
                            alertify.success(result.message)
                        })
                });
        }
        $this.init = function () {
            initializeGrid();
            initializeModalWithForm();
        };
    }
    $(function () {
        var self = new GeoRecordIndex();
        self.init();
    });

}(jQuery))
var _validFileExtensions = [".xls", ".xlsx", ".csv"];
var etc = '[...]';
function selectFile(maxLength) {
    $('.errorfile').html("");
    maxLength = 6;
    var fake = document.getElementById('fakefile');
    var input = document.getElementById('ImportedFile');
    var ext = input.value.split('.').pop().toLowerCase();

    if ($.inArray(ext, ['xls', 'xlsx', 'csv']) == -1) {
        input.value = '';

        $('.errorfile').html("Invalid extensions, allowed extensions are: " + _validFileExtensions.join(", "));

        return false;
    }

    if (Math.round(input.files[0].size / (1024 * 1024)) > 4) {


        input.value = '';
        $('.errorfile').html("Max Upload image size is 4MB only");

        return false;
    }
    else {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var imagePreviewId = $('#ImageUpload').attr('src', e.target.result);

                $('#divSelectedFile').attr('title', input.files[0].name);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
    var file = basename(input.value);

    fake.innerHTML = extractLabel(file, maxLength);


}
function basename(path) {
    return path.replace(/\\/g, '/').split('/').pop();
}
function getExtension(filename) {
    return filename.split('.').pop();
}

function extractLabel(filename, maxlength) {
    var ret = filename;

    if (filename.length - etc.length > maxlength) {
        var extension = '.' + getExtension(filename);
        var uncuttable = extension.length + etc.length;

        ret = filename.substring(0, maxlength - uncuttable) + etc + extension;
    }

    return ret;
}

