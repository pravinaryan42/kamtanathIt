(function ($) {

    function LoadMap() {
        var $this = this;
        $this.init = function () {
      
   

            $(document).off("click", "#loadMapBtn").on("click", "#loadMapBtn", function () {
                if ($('#SelectedDistrict').val() == undefined || $('#SelectedDistrict').val() == null || $('#SelectedDistrict').val() == 'Select District' || $('#SelectedDistrict').val() == ''||
                    $('#SelectedBlock').val() == undefined || $('#SelectedBlock').val() == null || $('#SelectedBlock').val() == 'Select Block' || $('#SelectedBlock').val() == ''
                ) {
                    Global.Alert("Please select Block and GP to load map");
                    return false;
            }

            if ($('#SelectedGP').val() == 'Select GP') {
                $('#SelectedGP').val('');
            }

                $.getJSON(domain + "map/GetLocations?DistrictName=" + $('#SelectedDistrict').val() + "&BlockName=" + $('#SelectedBlock').val() + "&GPName=" + $('#SelectedGP').val(), function (data) {
                    if (data != null && data.length > 0) {
                        var firstPointMarkerLat = parseFloat(data[0].Latitude)
                        var firstPointMarkerLong = parseFloat(data[0].Longitude)
                        var center = { lat: firstPointMarkerLat, lng: firstPointMarkerLong };
                        // Create the map
                        var map = new google.maps.Map(document.getElementById('map'), {
                            zoom:15,
                            center: center,
                            mapTypeId: google.maps.MapTypeId.SATELLITE
                        });
                    }
                    else {
                        var center = { lat: 26.8467, lng: 80.9462 };
                        // Create the map
                        var map = new google.maps.Map(document.getElementById('map'), {
                            zoom: 15,
                            center: center,
                            mapTypeId: google.maps.MapTypeId.SATELLITE
                        });
                    }
                   
                    var markerIcon = {
                        url: domain + 'images/markerr-24.png', // Your icon URL
                        scaledSize: new google.maps.Size(30, 30), // Set the width and height of the marker
                    };
                    var markerGIcon = {
                        url: domain + 'images/marker-24.png', // Your icon URL
                        scaledSize: new google.maps.Size(30, 30), // Set the width and height of the marker
                    };
                    // Loop through the retrieved locations and create markers
                    data.forEach(function (location) {                   
                        var marker = new google.maps.Marker({
                            position: {
                                lat: parseFloat(location.Latitude), lng: parseFloat(location.Longitude)
                            },
                            map: map,
                            icon: location.ConnectionType == 'E' ? markerIcon : markerGIcon, // Set the custom icon
                            title: location.Name,
                            mapTypeId: google.maps.MapTypeId.SATELLITE
                        });
                    });
                }).fail(function () {
                    console.error("Error fetching locations.");
                });

            });

            $(document).off("change", "#SelectedDistrict").on("change", "#SelectedDistrict", function () {

                bindBlock();
            });
            $(document).off("change", "#SelectedBlock").on("change", "#SelectedBlock", function () {
                bindGP()
            });
            function bindBlock() {
                $('#SelectedBlock').empty();
                var districtId = $('#SelectedDistrict').val();
                $('#SelectedBlock').append($('<option/>', {
                    value: '',
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
                    value: '',
                    text: "Select GP"
                }));
                if (districtId != null && districtId != '' && districtId != 'select') {
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
        }
    }
    $(function () {
        var self = new LoadMap();
        self.init();
    });

}(jQuery))

