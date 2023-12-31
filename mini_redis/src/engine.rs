use std::collections::HashMap;
use crate::constants::{self, Response};

pub struct Engine{
    data: HashMap<String, String>,
}

impl Engine{
    pub fn new() -> Engine{
        Engine {data: HashMap::new()}
    }

    pub fn process_message(&mut self, message: &[u8]) -> Result<Response, String>{
        let splitted_parts = Engine::split(message);

        let data = match splitted_parts {
            Ok(parts) => parts,
            Err(message) => return Result::Err(message)
        };
            
        let result = match data.0 {
            constants::Methods::Get => self.get(data.1),
            constants::Methods::Add => self.add(data.1, data.2),
            constants::Methods::Remove => self.remove(data.1),
            constants::Methods::Update => self.update(data.1, data.2)
        };

        return Result::Ok(result);
    }

    fn get(&self, key: String) -> Response{
        match self.data.get(&key){
            Some(data) => Response { response_type: constants::ResponseType::Data, data: data.to_string() } ,
            None => Response { response_type: constants::ResponseType::NotFound, data: "Item Not Found".to_string() }
        }
    }

    fn add(&mut self, key: String, value: String) -> Response{
        let response = key.clone();
        self.data.insert(key, value);
        Response { response_type: constants::ResponseType::Data, data: response}
    }

    fn remove(&mut self, key: String) -> Response{
        self.data.remove(&key);
        Response { response_type: constants::ResponseType::Data, data: key}
    }

    fn update(&mut self, key: String, value: String) -> Response{
        let result = self.data.insert(key.clone(), value.clone());

        match result {
            Some(_) => Response { response_type: constants::ResponseType::Data, data: key },
            None => self.add(key, value)
        }
    }

    fn split(message: &[u8]) -> Result<(constants::Methods, String, String), String>{
        let mut state: usize = 0;
        let mut data : [[u8; 256]; 3] = [[0u8; 256]; 3];
        let mut temp: [u8; 256] = [0u8; 256];
        let mut index = 0;

        for i in message{
            if state == 3{
                break;
            }

            if *i == constants::SEPERATOR{
                data[state] = temp;
                state += 1;
                temp = [0u8; 256];
                index = 0;
                continue;
            }
            
            temp[index] = *i;
            index += 1;
        }

        if state != 3{
            return Result::Err("Invalid format".to_string());
        }

        let method = match data[0][0]  {
            1 => constants::Methods::Get,
            2 => constants::Methods::Add,
            3 => constants::Methods::Remove,
            4 => constants::Methods::Update,
            _ => return Result::Err("Ivalid Method Detected".to_string())
        };

        let key = String::from_utf8_lossy(&data[1]).to_string();
        let value = String::from_utf8_lossy(&data[2]).to_string();

        return Result::Ok((method, key, value));
    }
}