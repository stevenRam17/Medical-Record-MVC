var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblDataPhy').DataTable({
        "ajax": {
            "url": "/Administration/Physician/getall"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "collegeNumber", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "physicianSpecialties[, ].specialty.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="btn-group w-75">

                                <a href="/Administration/Physician/Upsert?id=${data}"
                                   class="btn btn-dark mx-2"> 
							    <i class="bi bi-pencil-square"></i>Edit</a>

							    <a onClick=Delete('/Administration/Physician/Delete/${data}') class="btn btn-danger mx-2">
							    <i class="bi bi-trash"></i>Delete</a>

                            </div
                            `
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
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}

