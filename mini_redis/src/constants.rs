pub enum Methods{
    Get = 1,
    Add = 2,
    Remove = 3,
    Update = 4
}

pub enum ResponseType{
    Data = 1,
    Error = 2
}

pub const SEPERATOR : u8 = 0x0;
pub const DEFAULT_PORT: i32 = 9009;
