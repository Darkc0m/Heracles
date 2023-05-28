struct data {
    u8 data[parent.size];
};

struct file{
    $ += 4;
    u32 size;
    if(size > 0){
        $ -= 8;
        data* d : u32;
    }
    $ = addressof(this) + 8;
};

struct arc {
    char magic[4];
    u32 headerSize;
    u16 version;
    u16 numFiles;
    u16 numFilePointers;
    padding[2];
    char magic2[16];
    file f[numFilePointers];
};

arc file @ 0x00;
