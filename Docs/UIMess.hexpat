struct string {
    char s[];
};

struct pointer16 {
    u16 peek [[hidden]];
    if(peek == 0){
        continue;
    }else{
        $ -= 2;
        string* relativeTextPointer16 : u16 [[pointer_base("pointer_offset")]];
    }
};

struct uiMess {
    pointer16 ptr[288];
};

fn pointer_offset(u16 address){
    return address;
};

uiMess file @ 0x00;
