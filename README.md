# RepairRequest
# Telegram Bot for Repair Requests

This bot replaces manual messaging with a manager: clients submit requests directly via Telegram, and the manager receives a complete, structured entry containing all necessary details.

## Features
- Main menu: services, pricing, contact information, and request submission
- Step-by-step request collection: name, phone number, and description of the issue
- Phone number validation
- Data confirmation before submission ("Is everything correct?")
- Option to cancel the submission process at any stage
- Instant notifications sent to the manager via Telegram
- Storage of all requests, including phone numbers, dates, and statuses

## Suitable for
Repair services, auto shops, cleaning services, beauty salons — any business that handles client inquiries. The menu and text content can be customized to fit your specific needs.

## Technologies
C# / .NET, Telegram.Bot library; requests are stored in a text file.

## Getting Started
1. Obtain a token from @BotFather
2. Set the following environment variables:
   - BOT_TOKEN — your bot's token
   - ADMIN_CHAT_ID — the manager's chat ID
3. Run the command `dotnet run`

## Limitations
Incomplete requests are stored in memory and will be cleared if the bot restarts.
For a production-ready version, the system can be migrated to use a database.