function cancelar(ReservaId) {

    Swal.fire({
        title: "¿Estás seguro?",
        text: `¿Deseas cancelar la reserva?`,
        icon: "info",
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
                        Swal.fire("Cancelacion", response.message, "success").then((result) => {
                            if (result) {
                                window.location.reload()
                            }
                        });
                    } else {
                        Swal.fire("Error", response.message, "error");
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire("Error", "Ocurrió un error: " + error, "error");
                }
            });
        } else {
            Swal.fire("No se ha cambiado el estado de la persona.", "", "info");
        }
    });
}