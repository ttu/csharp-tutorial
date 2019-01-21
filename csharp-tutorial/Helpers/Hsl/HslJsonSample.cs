namespace csharp_tutorial.Helpers.Hsl
{
    public class HslJsonSample
    {
        // GET: http://dev.hsl.fi/siriaccess/vm/json?operatorRef=HSL&lineRef=1058
        public static string Json = @"
{
    'Siri': {
        'version': '1.3',
        'ServiceDelivery': {
            'ResponseTimestamp': 1393053418151,
            'ProducerRef': {
                'value': 'HSL'
            },
            'Status': true,
            'MoreData': false,
            'VehicleMonitoringDelivery': [{
                'version': '1.3',
                'ResponseTimestamp': 1393053418151,
                'Status': true,
                'VehicleActivity': [{
                    'ValidUntilTime': 1393053422091,
                    'RecordedAtTime': 1393053392091,
                    'MonitoredVehicleJourney': {
                        'LineRef': {
                            'value': '1058'
                        },
                        'DirectionRef': {},
                        'FramedVehicleJourneyRef': {
                            'DataFrameRef': {
                                'value': '2014-02-22'
                            }
                        },
                        'OperatorRef': {
                            'value': 'HSL'
                        },
                        'Monitored': true,
                        'VehicleLocation': {
                            'Longitude': 24.91796,
                            'Latitude': 60.18979
                        },
                        'VehicleRef': {
                            'value': '3606'
                        }
                    }
                }, {
                    'ValidUntilTime': 1393053420539,
                    'RecordedAtTime': 1393053390539,
                    'MonitoredVehicleJourney': {
                        'LineRef': {
                            'value': '1058'
                        },
                        'DirectionRef': {},
                        'FramedVehicleJourneyRef': {
                            'DataFrameRef': {
                                'value': '2014-02-22'
                            }
                        },
                        'OperatorRef': {
                            'value': 'HSL'
                        },
                        'Monitored': true,
                        'VehicleLocation': {
                            'Longitude': 24.87599,
                            'Latitude': 60.20583
                        },
                        'VehicleRef': {
                            'value': '3607'
                        }
                    }
                }, {
                    'ValidUntilTime': 1393053447722,
                    'RecordedAtTime': 1393053417722,
                    'MonitoredVehicleJourney': {
                        'LineRef': {
                            'value': '1058'
                        },
                        'DirectionRef': {},
                        'FramedVehicleJourneyRef': {
                            'DataFrameRef': {
                                'value': '2014-02-22'
                            }
                        },
                        'OperatorRef': {
                            'value': 'HSL'
                        },
                        'Monitored': true,
                        'VehicleLocation': {
                            'Longitude': 25.07629,
                            'Latitude': 60.20953
                        },
                        'VehicleRef': {
                            'value': '3608'
                        }
                    }
                }, {
                    'ValidUntilTime': 1393053442526,
                    'RecordedAtTime': 1393053412526,
                    'MonitoredVehicleJourney': {
                        'LineRef': {
                            'value': '1058'
                        },
                        'DirectionRef': {},
                        'FramedVehicleJourneyRef': {
                            'DataFrameRef': {
                                'value': '2014-02-22'
                            }
                        },
                        'OperatorRef': {
                            'value': 'HSL'
                        },
                        'Monitored': true,
                        'VehicleLocation': {
                            'Longitude': 24.96961,
                            'Latitude': 60.18871
                        },
                        'VehicleRef': {
                            'value': '3609'
                        }
                    }
                }, {
                    'ValidUntilTime': 1393053441755,
                    'RecordedAtTime': 1393053411755,
                    'MonitoredVehicleJourney': {
                        'LineRef': {
                            'value': '1058'
                        },
                        'DirectionRef': {},
                        'FramedVehicleJourneyRef': {
                            'DataFrameRef': {
                                'value': '2014-02-22'
                            }
                        },
                        'OperatorRef': {
                            'value': 'HSL'
                        },
                        'Monitored': true,
                        'VehicleLocation': {
                            'Longitude': 25.0314,
                            'Latitude': 60.19454
                        },
                        'VehicleRef': {
                            'value': '3610'
                        }
                    }
                }, {
                    'ValidUntilTime': 1393053447720,
                    'RecordedAtTime': 1393053417720,
                    'MonitoredVehicleJourney': {
                        'LineRef': {
                            'value': '1058'
                        },
                        'DirectionRef': {},
                        'FramedVehicleJourneyRef': {
                            'DataFrameRef': {
                                'value': '2014-02-22'
                            }
                        },
                        'OperatorRef': {
                            'value': 'HSL'
                        },
                        'Monitored': true,
                        'VehicleLocation': {
                            'Longitude': 24.88326,
                            'Latitude': 60.19684
                        },
                        'VehicleRef': {
                            'value': '3612'
                        }
                    }
                }]
            }]
        }
    }
}";
    }
}