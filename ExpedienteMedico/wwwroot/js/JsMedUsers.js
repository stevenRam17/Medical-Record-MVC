var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataPatient').DataTable({
        order: [[4, 'desc']],
        "ajax": {
            "url": "/User/User/getallmedical"
        },
        "columns": [
            { "data": "userId", "width": "15%" },
            { "data": "completeName", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "lastDateAttended", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="btn-group w-25 h-25">

                                <a href="/User/User/Edit?id=${data}"
                                   class="btn btn-primary mx-2"> 
							    <i class="bi bi-pencil-square"></i>Edit</a>

                                <a href="/Medical/MedicalHistory/Upsert?id=${data}"
                                   class="btn btn-primary mx-2">
							    <i class="bi bi-clipboard2-pulse"></i></i>Attend</a>

                            </div
                            `;
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(_url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: _url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
