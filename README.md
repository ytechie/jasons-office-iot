# jasons-office-iot

A couple of projects for automating my office.

Right now it just reads data from a Raspberry Pi with a FEZ Hat attached. It gathers temperature and light data, sends it to IoT hub, and uses Azure Stream Analytics to send the data to SQL Database. PowerBI is used to visualize the data.