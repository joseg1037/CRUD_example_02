function NamenLesen() {
    var autocode = $("#autocode").val();

    $.ajax({
        type: 'POST',
        url: '/Auto/NamenLesen',
        data:
        {
            autocode: autocode
        },
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#Marke").val(data.automarke);

            $("#kilometerstandId").val(data.autokilometerstand);

            $("#Herstellungsdatum").val(data.autoherstellungsdatum);

            $('#Fk_Id_Besitzer').val(data.Fk_Id_Besitzer);
            $('#Fk_Id_Besitzer').trigger("chosen:updated");

        },
        error: function (data) {
            alert("Error: " + data);
        },
    });
}
