u32 numEntries in;

struct string {
    char s[];
};

struct pointer16 {
    string* pointer32 : u16 [[pointer_base("pointer_offset")]];
};

struct entries {
    u16 peek [[hidden]];
    if(peek != 0x1100){
        continue;
    }else{
        pointer16 ptr;
    }
};

struct tutorial {
    entries entries[numEntries];
};

fn pointer_offset (u32 address){
    return address;
};

tutorial file @ 0x00;
