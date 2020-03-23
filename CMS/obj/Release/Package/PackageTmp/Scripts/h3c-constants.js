var StatusTypeJS = {
    Error: 0,
    Success: 1,
    Warning: 2
};
var dialogOpts = {
    autoOpen: false,
    height: "auto",
    width: 600,
    resizable: false,
    modal: true,
    buttons: {
        "Save": {
            text: "Submit",
            "class": 'btn btn-success',
            click: function () {
                submitForm($(this));
            }
        },
        "Cancel": {
            text: "Cancel",
            "class": 'btn btn-success',
            click: function () {
                $(this).dialog("close");
            }
        }
    }
};
$.GlobalMessage = {
    MESSAGE_UPDATE_CREDIT_SUCCESS: "cập nhật credit thành công",
    MESSAGE_REQUIRE_KEYWORD: "Bạn cần nhập keyword",
    MESSAGE_UPDATE_SUCCESS: "cập nhật thành công",
    REQUIRE_UPLOAD_FILE: "Bạn chưa upload file",
    UPLOAD_WRONG_FORMAT: "Định dạng file không hợp lệ",
    FILE_CONTENT_EMPTY: "Nội dung file trống",
    MESSAGE_SYSTEM_ERROR:"có lỗi xảy ra trong quá trình thực hiện"
};
$.H3CGlobalVariable = {
    ReturnSuccess: "success",
    ReturnFailed: "failed",
    SystemTitle: "CMS 123Thue",
    WaringSelectRow: "Vui lòng chọn 1 bản ghi từ database",
    WaringSelectKey: "Hãy chọn ít nhất 1 keyword",
    SysInfor: "Message from system",
    SysAlert: "update success",
    delfailed: "delete record not success",
    SysConfirm: "requiment confirm from system",
    SysQuestion: "do you sure delete this record ?",
    SysQuestionClose: "bạn có muốn đóng dự án này không ?",
    SysQuestions: "Bạn có chắc chắn muốn xóa nhiều bản ghi (s) ?",
    SysQuestionLock: "Bạn có chắc chắn muốn khóa bản ghi (s) ?",
    SysQuestionUnLock: "Bạn có chắc chắn muốn mở khóa bản ghi(s) ?",
    SystemAddRow:"Có lỗi xảy ra !",
    ErrorDeleteParentOrAnother: "Sorry, we've problem. Maybe you've deleted the Parent object. Please check again, if not, please contact your administrator.",
    SysError: "Đã có lỗi trong quá trình xử lý, liên hệ phòng kỹ thuật để giải quyết",
    DelayFunction: "Chức năng đang được kỹ thuật phát triển. Vui lòng quay lại sau"
};