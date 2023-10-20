
PRODUCT_TABLE = {
    'apple_iphone': 120.45,
    'android_phone': 99.50,
    'apple_tablet': 75.69,
    'android_tablet': 65.73,
    'windows_tablet': 51.49
}
DAYS_OF_WEEK = [
    'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'
]
PRODUCT_OPTIONS = {
    1: 'apple_iphone',
    2: 'android_phone',
    3: 'apple_tablet',
    4: 'android_tablet',
    5: 'windows_tablet'
}


def display_products():
    """Display the list of products."""
    for key, value in PRODUCT_OPTIONS.items():
        print(f"\n {key}= {value.replace('_', ' ').title()}")


def get_profit_for_period(days):
    """Calculate profit for the specified days."""
    daily_profit = 0
    for day in days:
        print(day)
        while True:
            display_products()
            product_category = int(input("Enter product number 1-5, or enter 0 to stop:  "))
            if product_category == 0:
                break

            if product_category in PRODUCT_OPTIONS:
                quantity = int(input("Enter quantity sold:  "))
                daily_profit += PRODUCT_TABLE[PRODUCT_OPTIONS[product_category]] * quantity
            else:
                print("Product category should be 0-5, no other number")
    return daily_profit


def main():
    """Main execution function."""
    print("\n Welcome to Circle Phones Profit calculator.")
    print("\n 1= One Day \n 2= One Week \n 3= Week of Business Days \n 4= Weekend")

    sales_period = int(input("Enter time period for sales data [1-4]:  "))

    if sales_period == 1:
        specific_day = input(f"Enter a specific day {DAYS_OF_WEEK}")
        print('For:', specific_day)
        daily_profit = get_profit_for_period([specific_day])

    elif sales_period == 2:
        daily_profit = get_profit_for_period(DAYS_OF_WEEK)

    elif sales_period == 3:
        daily_profit = get_profit_for_period(DAYS_OF_WEEK[:5])

    elif sales_period == 4:
        daily_profit = get_profit_for_period(DAYS_OF_WEEK[5:])

    else:
        print("Invalid sales period")
        return

    print(f'Total Profit in $CDN for the period is: {daily_profit}')
    if daily_profit >= 10000:
        print('You did well this period! Keep up the great work!')
    else:
        print('We did not reach our goal for this period. More work is needed.')

if __name__ == "__main__":
    main()