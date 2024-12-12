function toggleEstado(ReservaId) {

    Swal.fire({
        title: "¿Estás seguro?",
        text: `¿Deseas cancelar la persona?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí",
        cancelButtonText: "No",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Reserva/CancelarReserva',
                type: 'POST',
                data: { ReservaId: ReservaId },
                success: function (response) {
                    if (response.success) {
                        location.reload();
                    } else {
                        Swal.fire("Error", response.message, "error");
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire("Error", "Ocurrió un error: " + error, "error");
                }
            });
        } else {
            Swal.fire("No se ha cancelado la reserva.", "", "info");
        }
    });
}