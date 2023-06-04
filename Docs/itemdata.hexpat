struct string {
    char s[];
};

struct pointer16 {
    string* relativeTextPointer16 : u16 [[pointer_base("itemdata_pointer_offset")]];
};

struct itemdata {
    u32 numItems;
    u32 textSize;
    pointer16 ptr[numItems * 2];
};

fn itemdata_pointer_offset(u16){
    return parent.parent.numItems * 4 + 8;
};

itemdata file @ 0x00;
