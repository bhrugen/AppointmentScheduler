$(document).ready(function () {
    InitializeCalendar();
});

function InitializeCalendar() {
    try {
        $('#calendar').fullCalendar({
            timezone: false,
            header: {
                left: 'prev,next,today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            selectable: true,
            editable: false,
            select: function (event) {
                onShowModal(event, null);
            }


        });
    }
    catch (e) {
        alert(e);
    }

}


function onShowModal(obj, isEventDetail) {
    $("#appointmentInput").modal("show");
}