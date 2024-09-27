$(document).ready(function () {
    $(".is-active-checkbox").change(function () {
        var userId = $(this).data("userid");
        var isActive = $(this).is(":checked");

        $.ajax({
            url: "/User/ToggleIsActive",
            type: "POST",
            data: { userId: userId, isActive: isActive },
            success: function () {
                alert("User status updated successfully");
            },
            error: function () {
                alert("Error updating user status");
            }
        });
    });
});