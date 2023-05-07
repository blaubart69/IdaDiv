use std::{cmp::Ordering, env};

fn main() {
    let args  = env::args().skip(1).collect();
    
    let (dividend,divisor) =
        match read_args(&args) {
            Err(e) => { eprintln!("{}",e); std::process::exit(8); },
            Ok(r) => r
        };

    let (result,temps) = divide(&dividend, &divisor);
    print_calculation(&dividend, &divisor, &result, &temps);
}

fn divrem(a : i64, b: i64 ) -> (i64,i64) {
    let intdiv = a / b;
    let rem = a - ( b * intdiv );
    (intdiv,rem)
}

fn divrem_str(a : &str, b : &str) -> (char,String) {
    let a_num = a.parse::<i64>().unwrap();
    let b_num = b.parse::<i64>().unwrap();

    let (intdiv,rest) = divrem(a_num, b_num);

    (   char::from_digit(intdiv as u32, 10).unwrap()
      , rest.to_string()  )
}

fn divide(dividend : &str, divisor : &str) -> (String,Vec<String>) {

    let mut dividend_digits = dividend.chars();
    //
    // grab as many digits as the divisor has
    //
    let mut temp = dividend_digits.by_ref().take(divisor.len()).collect::<String>();
    //
    // if first divisor.len() digits are less than divisor, grab one more digit from dividend
    //
    if temp.as_str().cmp(divisor) == Ordering::Less {
        temp.push( dividend_digits.next().unwrap() );
    }

    //let divisor_int = divisor.parse::<i64>().unwrap();
    let mut result = String::new();
    let mut temp_results = Vec::<String>::new();

    loop {
        let (resultdigit,rest) = divrem_str(temp.as_str(), divisor);
        result.push(resultdigit);
        temp = rest;
        
        let herunter = match dividend_digits.next() {
            None => 'R',
            Some(next_digit) => next_digit
        };

        temp.push(herunter);
        temp_results.push(temp.clone());

        if herunter == 'R' {
            break;
        }
    }
    (result, temp_results)
}


fn print_calculation(dividend : &str, divisor : &str, result : &str, temps : &Vec<String>) {
    println!("{} : {} = {}", dividend, divisor, result);

    let  max_pad_len = dividend.len() + 1;
    let mut pad_len = max_pad_len - ( temps.len() - 1 );

    for temp in temps.iter() {
        println!("{: >1$}", temp, pad_len);
        pad_len += 1;
    }
    
}

fn read_args<'a>(args : &'a Vec<String>) -> Result<(&'a str,&'a str),String> {
    
    if args.len() != 2
    {
        return Err(format!("Bitte Divident und Divisor eingeben. {} Argumente gefunden.", args.len()) )
    }

    if  !(  is_all_digits(&args[0]) 
        &&  is_all_digits(&args[1])) 
    {
        return Err("Divident und Divisor müssen Zahlen sein".to_string())
    }

    let dividend = remove_leading_zeros(&args[0]);
    let divisor = remove_leading_zeros(&args[1]);

    if "0".eq(divisor)
    {
        return Err("Divison durch Null scheidet völlig aus".to_string())
    }
    else if    ( dividend.len()  < divisor.len() )
            || ( dividend.len() == divisor.len() && divisor.cmp(&dividend) == Ordering::Greater )
    {
        return Err("Dividend muß größer Divisor sein".to_string())
    }

    Ok((dividend,divisor))

}

fn remove_leading_zeros(number : &str) -> &str {
    if "0".eq(number) {
        number
    }
    else {
        number.trim_start_matches('0')
    }
}

fn is_all_digits(val : &str) -> bool
{
    val
    .chars()
    .all( |c| c.is_digit(10) )
}
