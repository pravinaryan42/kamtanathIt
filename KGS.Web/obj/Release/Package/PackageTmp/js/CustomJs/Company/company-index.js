(function ($) {
    function Index() {
        var $this = this, userLogin;
        function initializeForm() {
            InitCompnayKnockOut();
            companyform = new Global.FormHelper($("#formCompany"), {
                updateTargetId: "validation-summary", validateSettings: { ignore: '' }
            }, function onSucccess(result) {
                
                window.location.href = result.redirectUrl;
            });


            $("#PhoneNumber").on("keyup", function () {
                var valid = /^\d{0,10}(\.\d{0,2})?$/.test(this.value),
                    val = this.value;
                if (!valid) {
                    this.value = val.substring(0, val.length - 1);
                }
            });
            $(document).off("checked", ".rdPrimary").on("click", ".rdPrimary", function (event) {
                if ($(this).prop('checked')) {
                    $(this).val('B');
                }
              
            });

            
        }
      
        function InitCompnayKnockOut() {
        
            var $hdnFCompanyJson = $('#hdnCompanyData');
            function CompanyModel(CompanyId, CompanyName, GSTIN, FullAddress, PinCode, TaxDeductionType, MobileNumber, IsPrimary) {
                this.CompanyId = ko.observable(CompanyId);
                this.CompanyName = ko.observable(CompanyName);
                this.GSTIN = ko.observable(GSTIN);
                this.FullAddress = ko.observable(FullAddress);
                this.PinCode = ko.observable(PinCode);
                this.TaxDeductionType = ko.observable(TaxDeductionType);
                this.IsPrimary = ko.observable(IsPrimary);
                this.MobileNumber = ko.observable(MobileNumber);
            }


            // ViewModel for screen
            
            function ViewModel() {
                var self = this;
                var count = 0;
                $.getJSON(domain + "Company/GetUsersCompany", null, function (data) {
                    var array = [];
                    $.each(data, function (index, value) {
                        array.push(value);
                    });
                    if (array != undefined && array.length > 0) {
                        self.companylist(array);
                    }


                });
                self.companylist = ko.observableArray([
                    new CompanyModel(0, "", "", "", "","","","A"),
                ]);
              
                self.addCompany = function (viewModel, event) {
                    var companyValue = JSON.stringify(ko.toJS(self.companylist), null, 2);

                    count = (JSON.parse(companyValue)).length;
                    self.companylist.push(new CompanyModel(0, "", "", "", "", "", "", "A")); // Add a new person with no name  
                    //var editorArea = Array('Answer' + count);
                    //$.each(editorArea, function (i, editorArea) {
                    //    CKEDITOR.replace(editorArea, {});
                    //    attachEventCKEditor(editorArea);

                    //});
                };

                self.removeCompany = function (company, event) {
                    self.companylist.remove(company);


                };
                self.save = function () {

                    self.lastSavedJson(JSON.stringify(ko.toJS(self.companylist), null, 2));
                };
                self.lastSavedJson = ko.observable("")
                
                self.submitData = function (viewModel, event) {
                    
                    var companyValue = JSON.stringify(ko.toJS(self.companylist), null, 2);
                    var jsonObj = JSON.parse(companyValue);
                    var checkedPrimary = ($('input[type=radio][name=IsPrimary]:checked').attr('id'));
                    if (checkedPrimary == undefined || checkedPrimary == null || checkedPrimary == '') {
                        Global.Alert("Warning", "Please select primary company.");
                        return false;
                    }
                    checkedPrimary = checkedPrimary.replace('IsPrimary', '')
                    var chekedindex = (parseInt(checkedPrimary)) - 1;
                    $.each(jsonObj, function (index, value) {
                        if (index == chekedindex) {
                            value.IsPrimary = 'B';
                        }
                        else {
                            value.IsPrimary = 'A';
                        }
                       
                    });
                    $('#hdnCompanyData').val(JSON.stringify(jsonObj));
                    
                    $("#formCompany").submit();
                    //$("#formFaqData").on('click').trigger();
                    //$("#btn - submit").on('click').trigger();

                }

            }

            ko.cleanNode($('#companyformModel')[0]);
            //ko.applyBindings(viewModel, $('#IntakeFormConfimationModel')[0])
          
            ko.applyBindings(new ViewModel(), document.getElementById("companyformModel"));
        }




        $this.init = function () {
            initializeForm();

        };
    }

    $(function () {
        var self = new Index();
        self.init();
    });
}(jQuery));