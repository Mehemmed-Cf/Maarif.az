$.ajaxSetup({
    statusCode: {

        400: function () {
            alert("Data xetasi.Gonderilen datalari yoxlayin");
        },
        404: function () {
            alert("Data tapilmadi");
        }
    }
});


