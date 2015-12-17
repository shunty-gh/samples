#include <iostream>
#include <sstream>
#include "fizzbuzz.h"

FizzBuzz::FizzBuzz(int fizz, int buzz)
{
	_fizz = fizz;
	_buzz = buzz;
}

FizzBuzz::~FizzBuzz() {}


std::string const FizzBuzz::GetMessage(int value)
{
	bool isfb = false;
	std::ostringstream os;
	if (IsFizz(value))
	{
		os << "Fizz";
		isfb = true;
	}
	if (IsBuzz(value))
	{
		os << "Buzz";
		isfb = true;
	}
	if (!isfb)
	{
		os << value;
	}
	//os << std::endl;

	return os.str();
}

bool const FizzBuzz::IsFizz(int value)
{
	return (value % _fizz == 0);
}

bool const FizzBuzz::IsBuzz(int value)
{
	return (value % _buzz == 0);
}
